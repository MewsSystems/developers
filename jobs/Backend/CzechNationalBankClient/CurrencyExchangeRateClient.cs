using CzechNationalBankClient.Model;
using System.Text.Json;

namespace CzechNationalBankClient
{
    public class CurrencyExchangeRateClient : ICurrencyExchangeRateClient
    {
        private readonly HttpClient _httpClient;

        public CurrencyExchangeRateClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetCurrencyExchangeRatesAsync(string date, string language)
        {
            var response = await _httpClient.GetAsync($"cnbapi/exrates/daily?date={date}&lang={language}");
            response.EnsureSuccessStatusCode();
            var serialisedRates = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<CnbExchangeRateResponse>(serialisedRates);
            return rates?.ExchangeRates ?? Enumerable.Empty<CnbExchangeRate>();
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetOtherCurrencyExchangeRatesAsync(string month, string language)
        {
            var response = await _httpClient.GetAsync($"cnbapi/fxrates/daily-month?lang={language}&yearMonth={month}");
            response.EnsureSuccessStatusCode();
            var serialisedRates = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<CnbExchangeRateResponse>(serialisedRates);
            return rates?.ExchangeRates ?? Enumerable.Empty<CnbExchangeRate>();
        }
    }
}