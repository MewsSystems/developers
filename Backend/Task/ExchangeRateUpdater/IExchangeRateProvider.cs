using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates();
    }
}
