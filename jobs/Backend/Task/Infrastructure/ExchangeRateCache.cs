using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// In-memory cache implementation for exchange rates.
/// </summary>
public class ExchangeRateCache : IExchangeRateCache
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeRateCache> _logger;
    private readonly CnbExchangeRateConfiguration _configuration;
    private readonly HashSet<string> _cacheKeys = new();
    private readonly object _lock = new();
    private const string CacheKeyPrefix = "ExchangeRates_";

    public ExchangeRateCache(
        IMemoryCache cache,
        ILogger<ExchangeRateCache> logger,
        IOptions<CnbExchangeRateConfiguration> configuration)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IEnumerable<ExchangeRate>? GetCachedRates(IEnumerable<string> currencyCodes)
    {
        var cacheKey = GenerateCacheKey(currencyCodes);

        if (_cache.TryGetValue<List<ExchangeRate>>(cacheKey, out var cachedRates))
        {
            _logger.LogInformation(LogMessages.ExchangeRateCache.CacheHit,
                cacheKey, cachedRates?.Count ?? 0);
            return cachedRates;
        }

        _logger.LogInformation(LogMessages.ExchangeRateCache.CacheMiss, cacheKey);
        return null;
    }

    public void SetCachedRates(IEnumerable<ExchangeRate> rates)
    {
        if (rates == null)
        {
            throw new ArgumentNullException(nameof(rates));
        }

        var ratesList = rates.ToList();
        if (!ratesList.Any())
        {
            _logger.LogWarning(LogMessages.ExchangeRateCache.AttemptedCacheEmpty);
            return;
        }

        var currencyCodes = ratesList.Select(r => r.SourceCurrency.Code);
        var cacheKey = GenerateCacheKey(currencyCodes);

        var cacheDuration = TimeSpan.FromMinutes(_configuration.CacheDurationMinutes);
        var slidingExpiration = TimeSpan.FromMinutes(Math.Max(1, _configuration.CacheDurationMinutes / 2.0));

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration,
            SlidingExpiration = slidingExpiration
        };

        _cache.Set(cacheKey, ratesList, cacheOptions);

        // Track cache key for clearing
        lock (_lock)
        {
            _cacheKeys.Add(cacheKey);
        }

        _logger.LogInformation(
            LogMessages.ExchangeRateCache.CachedRates,
            ratesList.Count,
            cacheKey,
            _configuration.CacheDurationMinutes);
    }

    public void Clear()
    {
        lock (_lock)
        {
            foreach (var key in _cacheKeys)
            {
                _cache.Remove(key);
            }

            var count = _cacheKeys.Count;
            _cacheKeys.Clear();

            _logger.LogInformation(LogMessages.ExchangeRateCache.ClearedEntries, count);
        }
    }

    private static string GenerateCacheKey(IEnumerable<string> currencyCodes)
    {
        var sortedCodes = string.Join("_", currencyCodes.OrderBy(c => c));
        return $"{CacheKeyPrefix}{sortedCodes}";
    }
}
