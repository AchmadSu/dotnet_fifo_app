using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Interface.CacheInterface;

namespace FifoApi.Infrastructure.Cache.Sales
{
    public class SaleCacheHelper : ISaleCacheHelper
    {
        private readonly ICacheService _cache;
        private const string prefix = CacheKeys.Sales;
        public SaleCacheHelper(ICacheService cache)
        {
            _cache = cache;
        }
        public async Task InvalidateListAsync()
        {
            await _cache.RemoveByPrefixAsync($"{prefix}:list");
        }
    }
}