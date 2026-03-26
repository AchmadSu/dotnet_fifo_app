using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Interface.CacheInterface;

namespace FifoApi.Helpers.ProductHelper
{
    public class ProductCacheHelper : IProductCacheHelper
    {
        private readonly ICacheService _cache;
        private readonly string prefix = "products";
        public ProductCacheHelper(ICacheService cache)
        {
            _cache = cache;
        }
        public async Task InvalidateAllAsync(int id, string sku)
        {
            await InvalidateDetailAsync(id);
            await InvalidateSKUAsync(sku);
            await InvalidateListAsync();
        }

        public async Task InvalidateDetailAsync(int id)
        {
            await _cache.RemoveByPrefixAsync($"{prefix}:detail:{id}");
        }

        public async Task InvalidateListAsync()
        {
            await _cache.RemoveByPrefixAsync($"{prefix}:list");
        }

        public async Task InvalidateSKUAsync(string sku)
        {
            await _cache.RemoveByPrefixAsync($"{prefix}:sku:{sku}");

        }
    }
}