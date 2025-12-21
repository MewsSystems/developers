using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Decorator that adds caching capabilities to an exchange rate provider, specifically optimized
/// for CNB (Czech National Bank) data patterns. The cache expires based on CNB's fixing schedule
/// (14:30 Prague time on business days) with a configurable buffer.
/// </summary>
/// <param name="inner">The underlying exchange rate provider to decorate with caching.</param>
/// <param name="cache">The memory cache instance for storing exchange rate data.</param>
/// <param name="dateTimeProvider">The date and time provider for CNB-specific calculations.</param>
/// <param name="logger">Logger for tracking cache operations.</param>
internal class CnbCachedExchangeRateProviderDecorator(
    IExchangeRateProvider inner,
    IMemoryCache cache,
    ICacheDateTimeProvider dateTimeProvider,
    ILogger<CnbCachedExchangeRateProviderDecorator> logger)
    : IExchangeRateProvider
{
    /// <summary>
    /// Cache key for storing the latest valid-for date string.
    /// </summary>
    private const string LatestValidForKey = "ExchangeRates:LatestValidFor";
    
    /// <summary>
    /// Prefix for cache keys that store exchange rates by their valid-for date.
    /// </summary>
    private const string RatesKeyPrefix = "ExchangeRates:ValidFor:";
    
    /// <summary>
    /// Cache key for the loader lock to prevent concurrent loading of the same data.
    /// </summary>
    private const string LoaderKey = "ExchangeRates:LatestValid:Loader";

    /// <summary>
    /// Default buffer time added after the CNB fixing time before cache expiration.
    /// </summary>
    private static readonly TimeSpan DefaultBuffer = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Retrieves the latest exchange rates from cache if available, or loads them from the underlying provider.
    /// Cache expiration is based on CNB's fixing schedule (14:30 Prague time on business days).
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of cached
    /// or freshly loaded exchange rates.</returns>
    public async Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Attempting to retrieve exchange rates from cache");

        if (cache.TryGetValue<string>(LatestValidForKey, out var validForString) &&
            !string.IsNullOrWhiteSpace(validForString))
        {
            var ratesKey = RatesKeyPrefix + validForString;

            if (cache.TryGetValue<IReadOnlyList<ExchangeRate>>(ratesKey, out var cachedRates) &&
                cachedRates is { Count: > 0 })
            {
                logger.LogDebug("Cache hit: Retrieved {CachedRatesCount} exchange rates from cache for date {ValidFor}", 
                    cachedRates.Count, validForString);

                return cachedRates;
            }
        }

        logger.LogDebug("Loading exchange rates from underlying provider with cache loader");

        var loaded = await cache.GetOrCreateAsync<IReadOnlyList<ExchangeRate>>(LoaderKey, async entry =>
        {
            var absoluteExpirationUtc = dateTimeProvider.GetNextFixingExpirationUtc(DefaultBuffer);
            entry.AbsoluteExpiration = absoluteExpirationUtc;

            logger.LogDebug("Cache expiration set to: {ExpirationTime} UTC", absoluteExpirationUtc);

            var rates = await inner.GetLatestAsync(cancellationToken);
            if (rates.Count == 0)
            {
                logger.LogWarning("Underlying provider returned no exchange rates");
                return [];
            }

            var validFor = rates[0].ValidFor;
            var validForKeyPart = validFor.ToString("yyyy-MM-dd");

            logger.LogDebug("Loaded {RatesCount} exchange rates from provider, valid for: {ValidFor}",
                rates.Count, validFor);

            var ratesKey = RatesKeyPrefix + validForKeyPart;

            // Cache the rates
            cache.Set(
                ratesKey,
                rates,
                new MemoryCacheEntryOptions { AbsoluteExpiration = absoluteExpirationUtc });

            // Cache the latest valid-for date
            cache.Set(
                LatestValidForKey,
                validForKeyPart,
                new MemoryCacheEntryOptions { AbsoluteExpiration = absoluteExpirationUtc });

            logger.LogDebug("Successfully cached {RatesCount} exchange rates with key: {CacheKey}, expires: {ExpirationTime}",
                rates.Count, ratesKey, absoluteExpirationUtc);

            return rates;
        });

        return loaded ?? [];
    }
}
