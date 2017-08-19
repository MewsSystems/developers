using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> results = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {

                ReturnedCurrencies result = await GetAsyncSimple<ReturnedCurrencies>(String.Format(ConfigurationManager.AppSettings.Get("SourceAPI"), currency.Code), currency.Code);

                if (result != null)
                {
                    foreach (var item in result.rates)
                    {
                        results.Add(new ExchangeRate(currency, new Currency(item.Key), item.Value));
                    }
                }
            }

            return results;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        /// <summary>
        /// Here is take all resuls of our source code currency.
        /// The url of source you can find in app.config.
        /// </summary>
        private async Task<T> GetAsyncSimple<T>(string url, string currencyCode)
            where T : class
        {
            HttpClient httpClient = CreateHttpClient();
            try
            {
                string response = await httpClient.GetStringAsync(url);
                var result = await Task.Run(() => JsonConvert.DeserializeObject<T>(response));

                return result;
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not be found: {currencyCode}");
                return default(T);
            }
        }

        private class ReturnedCurrencies
        {
            public string @base { get; set; }
            public string date { get; set; }
            public Dictionary<string, decimal> rates { get; set; }

        }
    }
}
