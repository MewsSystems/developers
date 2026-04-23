using API.Models;
using ConsoleClient.Dtos;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;

        public APIClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currencyCodes = string.Join(",", currencies.Select(currency => currency.Code));
            var apiUrl = $"http://localhost:5032/api/ExchangeRate/CzechNationalBank?currencies={currencyCodes}";

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return ConvertJsonToExchangeRates(content);
        }

        private static List<ExchangeRate> ConvertJsonToExchangeRates(string json)
        {
            var exchangeRateDtos = JsonConvert.DeserializeObject<List<ExchangeRateDto>>(json);

            if (exchangeRateDtos is null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON data. The data may be invalid.");
            }

            var exchangeRates = exchangeRateDtos.Select(dto => new ExchangeRate(
                new Currency(dto.SourceCurrency.Code),
                new Currency(dto.TargetCurrency.Code),
                dto.Value
            )).ToList();

            return exchangeRates;
        }
    }
}
