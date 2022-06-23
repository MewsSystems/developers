using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Helpers;

public class CacheHelper
{
    private readonly IExchangeRateServiceSettings _settings;
    private readonly IMemoryCache _memoryCache;

    public CacheHelper(IMemoryCache memoryCache, IExchangeRateServiceSettings settings)
    {
        _settings    = settings;
        _memoryCache = memoryCache;
    }

    internal void SetCache(string key, object value, TimeSpan expiryTime)
    {
        if (!_settings.UseInMemoryCache)
            return;

        DateTime cacheExpiry = GetCacheExpiry(expiryTime, _settings.TimezoneId);
        
        var memoryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = cacheExpiry.GetDateTimeOffsetWithTimezoneId(_settings.TimezoneId)
        };
        _memoryCache.Set(key, value, memoryOptions);
    }

    internal T? GetCache<T>(string key)
    {
        if (!_settings.UseInMemoryCache)
            return default;

        return _memoryCache.Get<T>(key);
    }

    private static DateTime GetCacheExpiry(TimeSpan ts, string timezone)
    {
        var currentTime = DateTime.UtcNow.ConvertTimeFromUtcWithTimezoneId(timezone);
        var expiryDate = DateTime.Today.Add(ts);
        
        return expiryDate < currentTime 
                   ? expiryDate.AddDays(1)
                   : expiryDate;
    }
}