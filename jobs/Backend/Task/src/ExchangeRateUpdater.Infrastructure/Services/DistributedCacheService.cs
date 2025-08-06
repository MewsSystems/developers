using ExchangeRateUpdater.Domain.Services;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class DistributedCacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var cachedValue = await cache.GetStringAsync(key);
        if (cachedValue == null) return null;
        
        try
        {
            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value,  DateTimeOffset? absoluteExpiration, TimeSpan? absoluteExpirationRelativeNow,TimeSpan? slidingExpiration) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeNow, 
            SlidingExpiration = slidingExpiration,
            AbsoluteExpiration = absoluteExpiration,
        };
        
        var serializedValue = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, serializedValue, options);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var cachedValue = await cache.GetStringAsync(key);
        return !string.IsNullOrEmpty(cachedValue);
    }
} 