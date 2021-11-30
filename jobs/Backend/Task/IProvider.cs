using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
