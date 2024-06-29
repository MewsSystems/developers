using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        // I'm supposing that, if it's just passing currencies, it will always be checked against CZK. Adding it here as default just
        // in case in the future is needed to support other currency pairs.
        private static Currency DEFAULT_CURRENCY = new Currency("CZK");
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // .Result will make the call synchronous. I didn't want to modify the provided methods signature and for
            // this case we will need to get the information synchronously anyways.
            ExchangeRatesDictionary exchangeList = CreateApiProvider().FetchRates().Result;

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (var initialCurrency in currencies)
            {
                Currency targetCurrency = DEFAULT_CURRENCY;

                var value = exchangeList.GetRate(initialCurrency.Code, targetCurrency.Code);

                if (value != null)
                {
                    exchangeRates.Add(new ExchangeRate(initialCurrency, targetCurrency, value.Value));
                }
            }

            return exchangeRates;
        }

        protected virtual CZKExchangeRateApiProvider CreateApiProvider()
        {
            return new CZKExchangeRateApiProvider();
        }
    }
}
