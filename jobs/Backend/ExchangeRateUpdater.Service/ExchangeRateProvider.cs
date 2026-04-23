using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Service
{
    /// <summary>
    /// Provides methods for retrieving exchange rates for specified currencies from an external source.
    /// </summary>
    /// <param name="exchangeRateClient">The client used to obtain exchange rate data from an external provider.</param>
    /// <param name="logger">The logger used to record informational messages and errors related to exchange rate retrieval.</param>
    public class ExchangeRateProvider(IExchangeRateClient exchangeRateClient, ILogger<ExchangeRateProvider> logger, IMemoryCache cache) : IExchangeRateProvider
    {
        private const string CacheKey = "CnbRates";

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            ArgumentNullException.ThrowIfNull(currencies);

            // Try get from cache first
            if (!cache.TryGetValue<IEnumerable<ExchangeRate>>(CacheKey, out var cachedRates))
            {
                logger.LogInformation("Cache miss for CNB rates, fetching from upstream");
                var fetched = await exchangeRateClient.GetExchangeRatesAsync();

                var cacheExpiration = CalculateCacheExpiration();
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheExpiration);

                cache.Set(CacheKey, fetched, options);
                cachedRates = fetched;
            }
            else
            {
                logger.LogDebug("Cache hit for CNB rates");
            }

            var currencySet = currencies.ToHashSet();
            var filteredRates = cachedRates.Where(rate => currencySet.Contains(rate.SourceCurrency));
            return filteredRates;
        }

        /// <summary>
        /// Calculates the cache expiration duration based on CNB API publication schedule.
        /// The CNB publishes new rates around 15:00 UTC on working days.
        /// This method schedules the cache to expire shortly after the next expected publication.
        /// </summary>
        /// <returns>A TimeSpan representing the cache duration (minimum 1 minute, maximum 24 hours).</returns>
        private static TimeSpan CalculateCacheExpiration()
        {
            var now = DateTime.UtcNow;
            var nextPublication = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0, DateTimeKind.Utc);
            
            if (now >= nextPublication)
            {
                // Already past today's publication time, schedule for next day
                nextPublication = nextPublication.AddDays(1);
            }

            var absoluteExpirationRelative = nextPublication - now;
            
            // Guard: ensure a minimum cache duration and reasonable maximum (24h)
            // Cache will refresh even on non-working days, but that's acceptable because 
            // 1 unnecessary request a day is not a problem for CNB.
            if (absoluteExpirationRelative < TimeSpan.FromMinutes(1)) 
                absoluteExpirationRelative = TimeSpan.FromMinutes(1);
            
            if (absoluteExpirationRelative > TimeSpan.FromHours(24)) 
                absoluteExpirationRelative = TimeSpan.FromHours(24);

            return absoluteExpirationRelative;
        }
    }
}
