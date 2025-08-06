using ExchangeRateUpdater.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class DistributedCacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        return await Task.FromResult(cache.Get<T>(key));
    }

    public async Task SetAsync<T>(string key, T value,  DateTimeOffset? absoluteExpiration, TimeSpan? absoluteExpirationRelativeNow,TimeSpan? slidingExpiration) where T : class
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeNow, 
            SlidingExpiration = slidingExpiration,
            AbsoluteExpiration = absoluteExpiration,
        };
        
         cache.Set(key, value, options);
         await Task.CompletedTask;
    }

    public async Task RemoveAsync(string key)
    {
        cache.Remove(key);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Task.FromResult(cache.TryGetValue(key, out _));
    }
} 