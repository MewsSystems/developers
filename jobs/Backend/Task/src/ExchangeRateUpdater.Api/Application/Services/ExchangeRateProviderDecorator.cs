using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Api.Application.Services;

public sealed class ExchangeRateProviderDecorator(
    IExchangeRateProvider inner,
    IMemoryCache cache,
    ILogger<ExchangeRateProviderDecorator> logger,
    TimeSpan cacheDuration,
    TimeSpan historicalCacheDuration) : IExchangeRateProvider
{
    private const string CacheKeyPrefix = "exchange-rates";
    private static readonly string LatestCacheKey = BuildCacheKey(null);

    private DateOnly _lastCachedUtcDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private readonly object _dateLock = new();

    public async Task<Result<IReadOnlyCollection<ExchangeRate>>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default)
    {
        EvictLatestOnDayRollover();

        var cacheKey = BuildCacheKey(date);

        if (cache.TryGetValue(cacheKey, out Result<IReadOnlyCollection<ExchangeRate>>? cached) && cached is not null)
        {
            logger.LogDebug("Cache hit for exchange rates with key {CacheKey}", cacheKey);
            return cached;
        }

        logger.LogDebug("Cache miss for exchange rates with key {CacheKey}", cacheKey);

        var result = await inner.GetDailyRatesAsync(date, cancellationToken);

        if (result.IsSuccess)
        {
            var ttl = IsHistoricalDate(date) ? historicalCacheDuration : cacheDuration;
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            };

            cache.Set(cacheKey, result, cacheOptions);
            logger.LogDebug("Cached exchange rates with key {CacheKey} for {Duration}", cacheKey, ttl);
        }

        return result;
    }

    private static bool IsHistoricalDate(DateOnly? date) =>
        date.HasValue && date.Value < DateOnly.FromDateTime(DateTime.UtcNow);

    private void EvictLatestOnDayRollover()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        lock (_dateLock)
        {
            if (today <= _lastCachedUtcDate)
                return;

            _lastCachedUtcDate = today;
        }

        cache.Remove(LatestCacheKey);
        
        logger.LogInformation(
            "New UTC day {Today} detected — evicted latest exchange rates cache", today);
    }

    private static string BuildCacheKey(DateOnly? date) =>
        $"{CacheKeyPrefix}-{date?.ToString("yyyy-MM-dd") ?? "latest"}";
}
