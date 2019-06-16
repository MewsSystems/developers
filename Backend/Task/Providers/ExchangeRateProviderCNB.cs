using ExchangeRateUpdater.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProviderCNB : IExchangeRateProvider
    {
        private readonly static HttpHelperClient _client = new Helpers.HttpHelperClient();

        private const string ENDPOINT_URL =
            "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Call API
            string content = _client.GetUrl(ENDPOINT_URL).Result;
            // Parse result
            var objRate = ExchangeProviderParser.ParseContent(content);
            // Filter based on selected currencies
            var filteredRates = objRate.RateItems
                                // Filtering
                                .Where(x => currencies.Any(c => c.Code.Equals(x.Code, System.StringComparison.InvariantCultureIgnoreCase)))
                                // Projection
                                .Select(n => new ExchangeRate(
                                                            new Currency(n.Code),
                                                            new Currency("CZK"),
                                                            n.Rate / n.Amount
                                                            ));

            return filteredRates;
        }
    }
}
