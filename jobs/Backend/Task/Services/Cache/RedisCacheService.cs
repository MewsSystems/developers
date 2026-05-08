using System;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeRateUpdater.Services.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var json = await _distributedCache.GetStringAsync(key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var json = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        });
    }
}