namespace ExchangeRateUpdater.Infrastructure.Data;

using Domain.Common;
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

    public T GetFromCache<T>(string key)
    {
        cache.TryGetValue(key, out T cachedResponse);
        logger.LogInformation(cachedResponse is null ? $"{key} not found in cache" : $"{key} found in cache");
        return cachedResponse;
    }

    public void SetCache<T>(string key, T value, TimeSpan absoluteExpiration)
    {
        logger.LogInformation("{Key} added to cache", key);
        cache.Set(key, value, new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(absoluteExpiration));
    }

    public void SetCache<T>(string key, T value)
    {
        var defaultExpiration = TimeSpan.FromHours(DefaultExpirationHours);
        SetCache(key, value, defaultExpiration);
    }

    public void ClearCache(string key)
    {
        logger.LogInformation("{Key} removed from cache", key);
        cache.Remove(key);
    }
}