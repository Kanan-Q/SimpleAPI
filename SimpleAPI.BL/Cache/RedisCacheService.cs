using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleAPI.BL.Cache
{
    public sealed class RedisCacheService(IDistributedCache _cache) : ICacheService
    {
        public async Task<T> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(data)) return default;
            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var data = JsonSerializer.Serialize(value);
            var opt = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(key, data, opt);
        }
    }
}
