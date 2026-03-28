using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Interface.CacheInterface;

namespace FifoApi.Infrastructure.Cache.Products
{
    public class ProductCacheHelper : IProductCacheHelper
    {
        private readonly ICacheService _cache;
        private const string prefix = CacheKeys.Products;
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

        public async Task<int> GetListVersionAsync()
        {
            var version = await _cache.GetTAsync<int?>($"{prefix}:list:version");
            if (version == null)
            {
                version = 1;
                await _cache.SetAsync($"{prefix}:list:version", version);
            }

            return version.Value;
        }

        public async Task IncrementListVersionAsync()
        {
            var version = await GetListVersionAsync();

            await _cache.SetAsync(
                $"{prefix}:list:version",
                version + 1
            );
        }
    }
}