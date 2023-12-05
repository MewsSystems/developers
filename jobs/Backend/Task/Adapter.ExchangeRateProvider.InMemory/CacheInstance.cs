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
            return Value;
        }
        private set
        {
            Value = value;
        }
    }

    public CacheInstance(IEnumerable<ExchangeRate> cachedRates)
    {
        Value = cachedRates;
        AccessedTime = DateTime.Now;
    }
}
