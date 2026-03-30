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
using FifoApi.Interface.KafkaInterface;
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
            _logger.LogInformation(
                "Kafka event received. Topic: {Topic}, Payload: {Payload}",
                topic,
                payload
            );

            using var scope = _scopeFactory.CreateScope();

            var dispatcher = scope.ServiceProvider
                .GetRequiredService<KafkaMessageDispatcher>();

            try
            {
                await dispatcher.DispatchAsync(topic, payload);

                _logger.LogInformation(
                    "Kafka event processed successfully. Topic: {Topic}",
                    topic
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kafka event processing failed. Topic: {Topic}, Payload: {Payload}",
                    topic,
                    payload
                );

                throw;
            }
        }
    }
}