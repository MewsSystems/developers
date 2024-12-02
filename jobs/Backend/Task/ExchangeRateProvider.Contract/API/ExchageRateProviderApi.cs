using ExchangeRateProvider.Contract.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Contract.API
{
    public class ExchageRateProviderApi
    {
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public ExchageRateProviderApi(string apiUrl, string apiKey, HttpClient? httpClient = null)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;
            _httpClient = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Gets the exchange rates. Throws an exception if status code does not indicate success
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ExchangeRate>?> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var content = new StringContent(JsonConvert.SerializeObject(currencies), Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync($"{_apiUrl}{_apiKey}", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(apiResponse);
            }
            
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public static IEnumerable<Currency> GetSupportedCurrencies()
        {
            return new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };
        }
    }
}
