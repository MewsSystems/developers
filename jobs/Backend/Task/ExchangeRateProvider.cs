using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ResponseParser responseParser;
        private readonly string exchangeRatesSource;

        public ExchangeRateProvider()
        {
            responseParser = new ResponseParser();
            exchangeRatesSource = ConfigurationManager.AppSettings.Get("ExchangeRatesSource");
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            using (var httpClient = new HttpClient())
            {
                var response = GetDataFromSource(httpClient).Result;

                response = response.Where(
                    fetchedCurrency => currencies.Any(
                        requestedCurrency => requestedCurrency.Code == fetchedCurrency.SourceCurrency.Code));

                return response;
            }
        }

        private async Task<IEnumerable<ExchangeRate>> GetDataFromSource(HttpClient httpClient)
        {
            var responseString = await httpClient.GetStringAsync(exchangeRatesSource);

            return responseParser.ParseResponseFromSource(responseString);
        }
    }
}
