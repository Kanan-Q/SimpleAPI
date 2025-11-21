using Microsoft.Extensions.Caching.Distributed;
using SimpleAPI.Core.Cache;
using System.Text.Json;

namespace SimpleAPI.Infrastructure.Service;
public sealed class RedisCacheService(IDistributedCache _cache) : ICacheService
{
public async Task<T> GetAsync<T>(string key)
{
    var data = await _cache.GetStringAsync(key).ConfigureAwait(false);
    if (string.IsNullOrWhiteSpace(data)) return default;
    return JsonSerializer.Deserialize<T>(data);
}

public async Task RemoveAsync(string key)
{
    await _cache.RemoveAsync(key).ConfigureAwait(false);
}

public async Task SetAsync<T>(string key, T value)
{
    var data = JsonSerializer.Serialize(value);
    var opt = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
    await _cache.SetStringAsync(key, data, opt).ConfigureAwait(false);
}
}