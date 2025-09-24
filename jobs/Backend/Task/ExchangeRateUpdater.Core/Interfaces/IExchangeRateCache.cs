using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateCache
{
    /// <summary>
    /// Gets cached exchange rates for the specified currencies
    /// </summary>
    /// <param name="currencies">The currencies to get cached rates for</param>
    /// <returns>Maybe containing cached exchange rates</returns>
    Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);

    /// <summary>
    /// Caches exchange rates
    /// </summary>
    /// <param name="rates">The rates to cache</param>
    /// <param name="cacheExpiry">How long to cache the rates for</param>
    Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry);

    Task ClearCache();
}
