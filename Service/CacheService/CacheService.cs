using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Interface.CacheInterface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FifoApi.Service.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }
        public async Task<T?> GetTAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(value!);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var endpoints = _redis.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);

                var keys = server.Keys(pattern: $"{prefix}*");

                foreach (var key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
        }

        public async Task SetAsync(string key, object value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(
                key,
                JsonConvert.SerializeObject(value),
                expiry ?? TimeSpan.FromMinutes(5)
            );
        }
    }
}