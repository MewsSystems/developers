using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Data;

/// <summary>
/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
/// some of the currencies, ignore them.
/// </summary>

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.cnb.cz/cnbapi/exrates/daily";

        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {

            var currentDate = DateTime.UtcNow;
            var dateString = currentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var exchangeRates = new List<ExchangeRate>();
            var response = await _httpClient.GetAsync($"{BaseUrl}?date={dateString}&lang=EN");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var ratesResponse = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);

                if (ratesResponse?.Rates != null)
                {
                    foreach (var rateData in ratesResponse.Rates)
                    {
                        if (currencies.Any(c => c.Code == rateData.CurrencyCode))
                        {
                            var sourceCurrency = new Currency("CZK");
                            var targetCurrency = new Currency(rateData.CurrencyCode);
                            var rate = rateData.Rate / rateData.Amount; // Adjusting rate based on amount

                            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate));
                        }
                    }
                }
            }

            return exchangeRates;
        }
    }
}
