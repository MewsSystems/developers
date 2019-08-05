using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface ICurrentExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetCurrentExchangeRates(IEnumerable<Currency> currencies);
    }
}