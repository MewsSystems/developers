using ExchangeRateUpdater.Domain.Entities;

namespace Adapter.ExchangeRateProvider.InMemory;

/// <summary>
/// Class representing a cache value of exchange rates.
/// </summary>
internal class CacheValue
{ 
    /// <summary>
    /// The cached value.
    /// </summary>
    public IEnumerable<ExchangeRate> Value { get; }

    /// <summary>
    /// Constructor to create a cache value.
    /// </summary>
    /// <param name="cachedRates">The exchange rates to be cached.</param>
    public CacheValue(IEnumerable<ExchangeRate> cachedRates)
    {
        Value = cachedRates;
    }
}
