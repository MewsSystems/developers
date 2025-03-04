using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Cache;

public interface ICacheProvider
{
    bool TryGetCache<T>(string key, out T? value);
    bool TrySetCache<T>(string key, T value, int ttlInSeconds);
}

public class CacheProvider(IMemoryCache cache, ILogger<CacheProvider> logger) : ICacheProvider
{
    public bool TryGetCache<T>(string key, out T? value)
    {
        value = default;
        try
        {
            if (cache.TryGetValue(key, out T? cachedValue))
            {
                value = cachedValue;
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error retrieving cache for key {key}: {ex.Message}");
        }

        return false;
    }


    public bool TrySetCache<T>(string key, T value, int ttlInSeconds)
    {
        try
        {
            if (!cache.TryGetValue(key, out _))
            {
                cache.Set(key, value,
                    TimeSpan.FromSeconds(ttlInSeconds));
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error setting cache for key {key}: {ex.Message}");
        }

        return false;
    }
}