using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private IProvider _provider;

        public ExchangeRateProvider()
        { }

        public ExchangeRateProvider(IProvider provider) => (_provider) = (provider);

        public void SetProvider(IProvider provider) => (_provider) = (provider);

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return _provider.GetExchangeRates(currencies);
        }
    }
}
