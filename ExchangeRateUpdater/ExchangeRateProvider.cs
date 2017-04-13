using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IBankApi _bankApi;

        public ExchangeRateProvider(IEnumerable<Currency> currencies, Currency baseCurrency)
        {
            _bankApi = new CurrencyLayerBankApi(currencies, baseCurrency);
        }

        /// <summary>
        /// Ctor for Dependency Injection
        /// </summary>
        public ExchangeRateProvider(IBankApi bankApi)
        {
            _bankApi = bankApi;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
            {
                throw new NullReferenceException();
            }

            if (currencies.Count() <= 0)
            {
                throw new ArgumentNullException("Currencies list is empty");
            }

            var rates = _bankApi.GetValues(currencies);

            return rates;
        }
    }
}
