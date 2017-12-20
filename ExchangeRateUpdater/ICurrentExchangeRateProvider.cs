using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface ICurrentExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetCurrentExchangeRate(IEnumerable<Currency> currencies);
    }
}
