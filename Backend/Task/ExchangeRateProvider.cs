using ExchangeRateProvider;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            IRateProvider rateProvider = RateProviderFactory.Create();

            IDictionary<string, decimal> exchangeRatesDictionary = rateProvider.GetExchangeRates();

            foreach (Currency currency in currencies)
            {
                if (exchangeRatesDictionary.ContainsKey(currency.Code))
                {
                    ExchangeRate exchangeRate = ExchangeRateFactory.Create(rateProvider.SourceCurrencyCode, currency, exchangeRatesDictionary[currency.Code]);

                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
