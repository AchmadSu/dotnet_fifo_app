using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using FifoApi.Infrastructure.Messaging.Events.Products;
using FifoApi.Infrastructure.Messaging.Events.Sales;
using FifoApi.Infrastructure.Messaging.Events.Stocks;
using FifoApi.Interface.CacheInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Models;

namespace FifoApi.Infrastructure.Messaging
{
    public class KafkaCustomerService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<KafkaCustomerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaCustomerService(
            IConfiguration config,
            ILogger<KafkaCustomerService> logger,
            IServiceScopeFactory scopeFactory
        )
        {
            _config = config;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Waiting for Kafka to be ready...");
            await Task.Delay(10000, stoppingToken);

            var config = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "fifoapi-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            IConsumer<Ignore, string>? consumer = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                    consumer.Subscribe(KafkaTopics.All);

                    _logger.LogInformation("Connected to Kafka");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Kafka not ready, retrying in 5s...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            if (consumer == null) return;

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);

                        if (result == null) continue;

                        await HandleMessage(result.Topic, result.Message.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Kafka consume error");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected processing error");
                    }
                }
            }
            finally
            {
                consumer.Close();
                consumer.Dispose();
            }
        }

        private async Task HandleMessage(string topic, string payload)
        {
            switch (topic)
            {
                case KafkaTopics.ProductCreated:
                    await HandleEventAsync<ProductCreatedEvent>(payload, async evt =>
                    {
                        if (evt == null) return;

                        using var scope = _scopeFactory.CreateScope();
                        var helper = scope.ServiceProvider
                            .GetRequiredService<IProductCacheHelper>();

                        await helper.IncrementListVersionAsync();
                        await helper.InvalidateListAsync();
                    });
                    break;

                case KafkaTopics.ProductUpdated:
                    await HandleEventAsync<ProductUpdatedEvent>(payload, async evt =>
                    {
                        if (evt == null) return;

                        using var scope = _scopeFactory.CreateScope();
                        var helper = scope.ServiceProvider
                            .GetRequiredService<IProductCacheHelper>();

                        await helper.IncrementListVersionAsync();
                        await helper.InvalidateAllAsync(evt.Id, evt.SKU);
                    });
                    break;

                case KafkaTopics.SaleCreated:
                    await HandleEventAsync<SaleCreatedEvent>(payload, async evt =>
                    {
                        if (evt == null) return;

                        using var scope = _scopeFactory.CreateScope();

                        var repo = scope.ServiceProvider
                            .GetRequiredService<IStockMovementRepository>();

                        var saleCache = scope.ServiceProvider
                            .GetRequiredService<ISaleCacheHelper>();

                        var exists = await repo.ExistsBySaleIdAsync(evt.Id);
                        if (exists) return;

                        await repo.CreateStockMovementsAsync(evt.Movements);
                        await saleCache.InvalidateListAsync();
                    });
                    break;

                case KafkaTopics.StockUpdated:
                    await HandleEventAsync<StockUpdatedEvent>(payload);
                    break;

                case KafkaTopics.StockMovementCreated:
                    await HandleEventAsync<StockMovementCreatedEvent>(payload);
                    break;

                default:
                    _logger.LogWarning("No handler for topic {Topic}", topic);
                    break;
            }
        }

        private async Task HandleEventAsync<T>(
            string payload,
            Func<T?, Task>? action = null
        )
        {
            try
            {
                var data = JsonSerializer.Deserialize<T>(payload);

                _logger.LogInformation("Event received: {@Event}", data);

                if (action != null)
                    await action(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process event");
            }
        }
    }
}