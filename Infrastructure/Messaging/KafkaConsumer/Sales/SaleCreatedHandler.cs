using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FifoApi.Infrastructure.Messaging.Events.Sales;
using FifoApi.Interface.CacheInterface;
using FifoApi.Interface.KafkaInterface;
using FifoApi.Interface.StockInterface;

namespace FifoApi.Infrastructure.Messaging.KafkaConsumer.Sales
{
    public class SaleCreatedHandler : IKafkaMessageHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public string Topic => KafkaTopics.SaleCreated;

        public SaleCreatedHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(string payload)
        {
            var evt = JsonSerializer.Deserialize<SaleCreatedEvent>(payload);
            if (evt == null) return;

            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider
                .GetRequiredService<IStockMovementRepository>();

            var exists = await repo.ExistsBySaleIdAsync(evt.Id);
            if (exists) return;

            var saleCache = scope.ServiceProvider
                .GetRequiredService<ISaleCacheHelper>();

            await repo.CreateStockMovementsAsync(evt.Movements);
            await saleCache.InvalidateListAsync();
        }
    }
}