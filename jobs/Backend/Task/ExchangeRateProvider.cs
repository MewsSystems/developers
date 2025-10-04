using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private DB rates;
        private LoadRates load;
        private List<ExchangeRate> exchangeRates;

        public ExchangeRateProvider()
        {
            rates = DB.GetInstance();
            exchangeRates = new();
            load = new APICall(new LoadData());
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            bool result = load.Load(string.Empty).Result;

            foreach (Currency currency in currencies)
            {
                if (rates.TryGetValue(currency.Code, out Rate rate))
                {
                    exchangeRates.Add(new(currency, new Currency("CZK"), rate.rate));
                }
            }

            return exchangeRates;
        }
    }
}
