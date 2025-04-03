using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRatesSource = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

            var client = new HttpClient();

            var response = client.GetAsync(exchangeRatesSource).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            var allExchangeRates = ExchangeRateParser.Parse(result);

            var ratesForCurrencies = allExchangeRates
                .Where(rate => currencies.Contains(rate.SourceCurrency) && currencies.Contains(rate.TargetCurrency));

            return ratesForCurrencies;
        }
    }
}
