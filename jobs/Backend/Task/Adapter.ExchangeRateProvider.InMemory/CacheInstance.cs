using ExchangeRateUpdater.Domain.Entities;

namespace Adapter.ExchangeRateProvider.InMemory;

internal class CacheInstance
{
    public DateTime AccessedTime { get; private set; }
    public IEnumerable<ExchangeRate> Value
    {
        get
        {
            AccessedTime = DateTime.Now;
            return _value;
        }
    }

    private IEnumerable<ExchangeRate> _value;

    public CacheInstance(IEnumerable<ExchangeRate> cachedRates)
    {
        _value = cachedRates;
        AccessedTime = DateTime.Now;
    }
}
