using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal class ExchangeRateProviderWithCaching : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        private IEnumerable<ExchangeRate> _cachedExchangeRates;

        public ExchangeRateProviderWithCaching(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(ContainsAllExchangeRates(currencies))
            {
                return _cachedExchangeRates;
            }

            return await _exchangeRateProvider.GetExchangeRates(currencies);
        }

        private bool ContainsAllExchangeRates(IEnumerable<Currency> currencies)
        {
            if(_cachedExchangeRates == null)
            {
                return false;
            }

            return currencies.All(c => _cachedExchangeRates.Contains(new ExchangeRate(c, Currency.CZK, DateTime.Today, 0, 0)));
        }
    }
}
