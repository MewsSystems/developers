using ExchangeRateUpdater.Domain.Entities;

namespace Adapter.ExchangeRateProvider.InMemory;

/// <summary>
/// Class representing a cache instance of exchange rates.
/// </summary>
internal class CacheInstance
{
    /// <summary>
    /// Last time the values was accessed. Used by the simple LRU use case.
    /// </summary>
    public DateTime AccessedTime { get; private set; }
    
    /// <summary>
    /// The cached value.
    /// </summary>
    public IEnumerable<ExchangeRate> Value
    {
        get
        {
            AccessedTime = DateTime.Now;
            return _value;
        }
    }

    private IEnumerable<ExchangeRate> _value;

    /// <summary>
    /// Constructor to create a cache instance.
    /// </summary>
    /// <param name="cachedRates">The exchange rates to be cached.</param>
    public CacheInstance(IEnumerable<ExchangeRate> cachedRates)
    {
        _value = cachedRates;
        AccessedTime = DateTime.Now;
    }
}
