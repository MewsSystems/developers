using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ExchangeRateProvider(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Arguably a null list is an exception to be logged, but context is everything. In this instance we'll just handle it gracefully. 
            if (currencies == null) return Enumerable.Empty<ExchangeRate>();


            // Could add a date to the function to get 
            var url = _apiSettings.ExchangeRatesBaseUrl + 
                $"{_apiSettings.DailyRatesEndpoint}?date={DateTime.UtcNow:yyyy-MM-dd}&lang={_apiSettings.LanguageCode}";

            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));

            if (!response.IsSuccessStatusCode) return Enumerable.Empty<ExchangeRate>();

            var content = await response.Content.ReadAsStringAsync();

            var bankRates = JsonConvert.DeserializeObject<DailyRatesDtoContainer>(content);

            return bankRates?.Rates.Where(r => currencies.Any(c => c.Code == r.CurrencyCode)).Select(r => ToExchangeRate(r)).ToList() ?? Enumerable.Empty<ExchangeRate>();
        }

        private static ExchangeRate ToExchangeRate(DailyRateDto rateDto)
        {
            return new ExchangeRate(new Currency("CZK"), new Currency(rateDto.CurrencyCode), rateDto.Rate);
        }
    }
}
