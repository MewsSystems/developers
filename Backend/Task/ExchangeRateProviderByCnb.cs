using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderByCnb : IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            throw new System.NotImplementedException();
        }
    }
}