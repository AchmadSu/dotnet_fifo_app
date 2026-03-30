using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FifoApi.Infrastructure.Messaging.Events.Products;
using FifoApi.Interface.CacheInterface;
using FifoApi.Interface.KafkaInterface;

namespace FifoApi.Infrastructure.Messaging.KafkaConsumer.Products
{
    public class ProductCreatedHandler : IKafkaMessageHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public string Topic => KafkaTopics.ProductCreated;

        public ProductCreatedHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(string payload)
        {
            var evt = JsonSerializer.Deserialize<ProductCreatedEvent>(payload);
            if (evt == null) return;

            using var scope = _scopeFactory.CreateScope();
            var helper = scope.ServiceProvider
                .GetRequiredService<IProductCacheHelper>();

            await helper.IncrementListVersionAsync();
            await helper.InvalidateListAsync();
        }
    }
}