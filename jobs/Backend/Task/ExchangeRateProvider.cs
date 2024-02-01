using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        }

        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rates "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        //public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        //{
        //    return Enumerable.Empty<ExchangeRate>();
        //}

        /// <summary>
        /// Asynchronously retrieves exchange rates among the specified currencies for a given date
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            var tasks = new List<Task>();

            //Currency currency1 = currencies.ElementAt(0);
            //Currency currency2 = currencies.ElementAt(2);
            //tasks.Add(FetchExchangeRateAsync(result, currency1, currency2, date));

            // Fetches the exchange rate for each pair for a given date
            foreach (var currency1 in currencies)
            {
                foreach (var currency2 in currencies)
                {
                    if (currency1 != currency2)
                    {
                        tasks.Add(FetchExchangeRateAsync(result, currency1, currency2, date));
                    }
                }
            }

            await Task.WhenAll(tasks);

            return result;
        }

        /// <summary>
        /// Asynchronously fetches the exchange rate between two currencies for a given date and adds it to the result list
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sourceCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private async Task FetchExchangeRateAsync(List<ExchangeRate> result, Currency sourceCurrency, Currency targetCurrency, DateTime date)
        {
            var rate = await GetExchangeRateAsync(sourceCurrency.Code.ToLower(), targetCurrency.Code.ToLower(), date);
            if (rate != null)
            {
                result.Add(new ExchangeRate(sourceCurrency, targetCurrency, Convert.ToDecimal(rate)));
            }
        }

        /// <summary>
        /// Asynchronously fetches the exchange rate between two currencies for a given date
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private async Task<string?> GetExchangeRateAsync(string from, string to, DateTime date)
        {
            try
            {
                string url;
                string baseUrl = "https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/";
                url = baseUrl + date.ToString("yyyy-MM-dd") + "/currencies/";
                url = url + from + "/" + to + ".json";

                var jsonString = await GetResponseAsync(url);
                return JObject.Parse(jsonString)[to].ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Asynchronously fetches a JSON response from a given URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<string> GetResponseAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonString;

                // Check if the response is compressed
                if (response.Content.Headers.ContentEncoding.Any(encoding => encoding.Equals("gzip", StringComparison.OrdinalIgnoreCase)))
                {
                    // Decompress it
                    using (var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    // Read the responce directly
                    jsonString = await response.Content.ReadAsStringAsync();
                }

                return jsonString;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve data from URL: {url}", ex);
            }
        }

        /// <summary>
        /// Asynchronously fetches exchange rates for specified currencies from the Czech National Bank API for a given date
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<ExchangeRate>?> GetCzechExchangeRateAsync(IEnumerable<Currency> currencies, DateTime date)
        {
            try
            {
                string url;
                string baseUrl = "https://api.cnb.cz/cnbapi/exrates/daily?";
                url = baseUrl + "date=" + date.ToString("yyyy-MM-dd") + "&lang=EN";

                var jsonString = await GetResponseAsync(url);
                var apiResponse = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(jsonString);

                if (apiResponse?.rates != null && apiResponse.rates.Count > 0)
                {
                    return apiResponse.rates
                        .Where(rate => currencies.Any(currency => currency.Code == rate.currencyCode))
                        .Select(rate =>
                            {
                                Currency sourceCurrency = new Currency(rate.currencyCode);
                                Currency targetCurrency = new Currency("CZK"); // Czech currency is the target
                                decimal value = rate.rate / rate.amount;

                                return new ExchangeRate(sourceCurrency, targetCurrency, value);
                            }).ToList();

                }

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
