namespace ExchangeRateUpdater.Infrastructure.Data;

using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class CacheRepository : ICacheRepository
{
    private const int DefaultExpirationHours = 1;

    private readonly IMemoryCache cache;
    private readonly ILogger<CacheRepository> logger;

    public CacheRepository(IMemoryCache cache, ILogger<CacheRepository> logger)
    {
        Ensure.Argument.NotNull(cache, nameof(cache));
        Ensure.Argument.NotNull(logger, nameof(logger));
        this.cache = cache;
        this.logger = logger;
    }

    public Task<Dictionary<string, ExchangeRate>?> GetRatesDictionaryAsync(string key)
    {
        cache.TryGetValue(key, out Dictionary<string, ExchangeRate>? cachedResponse);
        logger.LogInformation(cachedResponse is null ? $"{key} not found in cache" : $"{key} found in cache");
        return Task.FromResult(cachedResponse);
    }

    public Task<List<ExchangeRate>?> GetRatesListAsync(string key)
    {
        cache.TryGetValue(key, out List<ExchangeRate>? cachedResponse);
        logger.LogInformation(cachedResponse is null ? $"{key} not found in cache" : $"{key} found in cache");
        return Task.FromResult(cachedResponse);
    }

    public Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value, TimeSpan absoluteExpiration)
    {
        logger.LogInformation("{Key} added to cache", key);
        cache.Set(key, value, new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration));
        return Task.CompletedTask;
    }

    public Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value)
    {
        var defaultExpiration = TimeSpan.FromHours(DefaultExpirationHours);
        return SetRatesAsync(key, value, defaultExpiration);
    }

    public Task SetRatesListAsync(string key, List<ExchangeRate> value, TimeSpan absoluteExpiration)
    {
        logger.LogInformation("{Key} added to cache", key);
        cache.Set(key, value, new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration));
        return Task.CompletedTask;
    }

    public Task SetRatesListAsync(string key, List<ExchangeRate> value)
    {
        var defaultExpiration = TimeSpan.FromHours(DefaultExpirationHours);
        return SetRatesListAsync(key, value, defaultExpiration);
    }

    public Task ClearCacheAsync(string key)
    {
        logger.LogInformation("{Key} removed from cache", key);
        cache.Remove(key);
        return Task.CompletedTask;
    }
}