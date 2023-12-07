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
    public IEnumerable<ExchangeRate> Value 
    { 
        get 
        {
            return _value;
        } 
    }
    public DateTime LastAccessedTime { get; private set; }

    private IEnumerable<ExchangeRate> _value;
    private ReferenceTime _referenceTime;

    /// <summary>
    /// Constructor to create a cache value.
    /// </summary>
    /// <param name="cachedRates">The exchange rates to be cached.</param>
    public CacheValue(IEnumerable<ExchangeRate> cachedRates, ReferenceTime referenceTime)
    {
        _value = cachedRates;
        _referenceTime = referenceTime ?? throw new ArgumentNullException(nameof(referenceTime));
        LastAccessedTime = referenceTime.GetTime();
    }

    public void UpdateTime()
    {
        LastAccessedTime = _referenceTime.GetTime();
    }
}
