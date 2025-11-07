using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Interface for caching exchange rates.
/// </summary>
public interface IExchangeRateCache
{
    /// <summary>
    /// Gets cached exchange rates for the specified currencies.
    /// </summary>
    /// <param name="currencyCodes">Currency codes to retrieve.</param>
    /// <returns>Cached exchange rates, or null if not in cache or expired.</returns>
    IEnumerable<ExchangeRate>? GetCachedRates(IEnumerable<string> currencyCodes);

    /// <summary>
    /// Caches exchange rates with configured TTL.
    /// </summary>
    /// <param name="rates">Exchange rates to cache.</param>
    void SetCachedRates(IEnumerable<ExchangeRate> rates);

    /// <summary>
    /// Clears all cached exchange rates.
    /// </summary>
    void Clear();
}
