using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IBankApi
    {
        IEnumerable<ExchangeRate> GetValues(IEnumerable<Currency> currencies);
    }
}
