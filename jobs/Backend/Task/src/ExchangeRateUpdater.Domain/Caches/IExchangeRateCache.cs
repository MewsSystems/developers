using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Caches;

/// <summary>
/// Cache service for exchange rate.
/// </summary>
public interface IExchangeRateCache
{
    /// <summary>
    /// Sets exchange rates by each date.
    /// </summary>
    /// <param name="dateTime">The date.</param>
    /// <param name="exchangeRates">The exchange rates.</param>
    void Set(DateTime dateTime, IEnumerable<ExchangeRate>? exchangeRates);
    
    /// <summary>
    /// Gets latest exchanges rates.
    /// </summary>
    IEnumerable<ExchangeRate>? Get();
}