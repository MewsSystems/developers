using ExchangeRateUpdater.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.Services;

public class DistributedCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public DistributedCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        return await Task.FromResult(_cache.Get<T>(key));
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        
        _cache.Set(key, value, options);
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Task.FromResult(_cache.TryGetValue(key, out _));
    }
} 