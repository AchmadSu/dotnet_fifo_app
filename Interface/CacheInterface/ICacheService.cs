using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Interface.CacheInterface
{
    public interface ICacheService
    {
        Task<T?> GetTAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan? expiry = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefix);
    }
}