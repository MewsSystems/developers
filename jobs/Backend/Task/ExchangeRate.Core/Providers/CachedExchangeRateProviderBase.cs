using ExchangeRate.Core.Services;

namespace ExchangeRate.Core.Providers;

public abstract class CachedExchangeRateProviderBase
{
    protected readonly ICacheService CacheService;

    protected readonly string CacheKeyPrefix;

    protected CachedExchangeRateProviderBase(ICacheService cacheService, string cacheKeyPrefix)
    {
        CacheService = cacheService;
        CacheKeyPrefix = cacheKeyPrefix;
    }

    protected async Task<T?> GetCacheAsync<T>(string cacheKey)
    {
        return await CacheService.GetAsync<T>($"{CacheKeyPrefix}_{cacheKey}");
    }

    protected async Task<bool> SetCacheAsync<T>(string cacheKey, T value)
    {
        return await CacheService.SetAsync($"{CacheKeyPrefix}_{cacheKey}", value);
    }
}
