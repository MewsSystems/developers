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

            await Task.Run(() => Parallel.ForEach(currencies, currency =>
            {
                ReturnedCurrencies result = GetAsyncSimple<ReturnedCurrencies>(String.Format(ConfigurationManager.AppSettings.Get("SourceAPI"), currency.Code), currency.Code).Result;

                if (result != null)
                {
                    List<Currency> newListCurrency = currencies.ToList();

                    for (int i = newListCurrency.FindIndex(c => c.Code == currency.Code) - 1; i >= 0; i--)
                    {
                        newListCurrency.RemoveAt(i);
                    }
                    
                    foreach (var item in result.Rates.Where(r => newListCurrency.FirstOrDefault(c => c.Code == r.Key) != null))
                    {
                        results.Add(new ExchangeRate(currency, new Currency(item.Key), item.Value));
                    }
                }
                    
            }));
                              
            return results.OrderBy(x => x.SourceCurrency.Code);
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
            [JsonProperty(PropertyName = "base")]
            public string Base { get; set; }

            [JsonProperty(PropertyName = "date")]
            public string Date { get; set; }

            [JsonProperty(PropertyName = "rates")]
            public Dictionary<string, decimal> Rates { get; set; }

        }
    }
}
