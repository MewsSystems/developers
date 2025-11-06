using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Constants;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// In-memory cache implementation for supported currency codes.
/// </summary>
public class SupportedCurrenciesCache : ISupportedCurrenciesCache
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<SupportedCurrenciesCache> _logger;
    private readonly CnbExchangeRateConfiguration _configuration;
    private const string CacheKey = "SupportedCurrencies_All";

    public SupportedCurrenciesCache(
        IMemoryCache cache,
        ILogger<SupportedCurrenciesCache> logger,
        IOptions<CnbExchangeRateConfiguration> configuration)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IEnumerable<string>? GetCachedCurrencies()
    {
        if (_cache.TryGetValue<List<string>>(CacheKey, out var cachedCurrencies))
        {
            _logger.LogInformation(LogMessages.SupportedCurrenciesCache.CacheHit, cachedCurrencies?.Count ?? 0);
            return cachedCurrencies;
        }

        _logger.LogInformation(LogMessages.SupportedCurrenciesCache.CacheMiss);
        return null;
    }

    public void SetCachedCurrencies(IEnumerable<string> currencyCodes)
    {
        if (currencyCodes == null)
        {
            throw new ArgumentNullException(nameof(currencyCodes));
        }

        var currencyList = currencyCodes.ToList();
        if (!currencyList.Any())
        {
            _logger.LogWarning(LogMessages.SupportedCurrenciesCache.AttemptedCacheEmpty);
            return;
        }

        var cacheDuration = TimeSpan.FromMinutes(_configuration.CacheDurationMinutes);
        var slidingExpiration = TimeSpan.FromMinutes(Math.Max(1, _configuration.CacheDurationMinutes / 2.0));

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration,
            SlidingExpiration = slidingExpiration
        };

        _cache.Set(CacheKey, currencyList, cacheOptions);

        _logger.LogInformation(
            LogMessages.SupportedCurrenciesCache.CachedCurrencies,
            currencyList.Count,
            _configuration.CacheDurationMinutes);
    }

    public void Clear()
    {
        _cache.Remove(CacheKey);
        _logger.LogInformation(LogMessages.SupportedCurrenciesCache.ClearedCache);
    }
}
