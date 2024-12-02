using ExchangeRates.Core.Models;
using ExchangeRates.Core.Models.CnbApiResponse;
using ExchangeRates.Core.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRates.Core.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateSettings _settings;

        public ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRateSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // If no currencies are provided, return an empty list
            if (!currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            try
            {
                // Try to fetch exchange rates from the CNB API
                var response = _httpClient.GetAsync(_settings.CnbApiUrl).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request to {_settings.CnbApiUrl} failed with status code {response.StatusCode}.");
                }

                // Deserialize the response content
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var exchangeRatesResponse = JsonSerializer.Deserialize<CnbExchangeRatesResponse>(responseContent);

                if (exchangeRatesResponse == null || exchangeRatesResponse.Rates == null)
                {
                    throw new InvalidOperationException("Failed to deserialize exchange rates response.");
                }

                // Filter the exchange rates based on the provided currencies parameter
                // and map to ExchangeRate objects
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));
                var filteredRates = exchangeRatesResponse.Rates
                    .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
                    .Select(rate => new ExchangeRate(
                        new Currency(rate.CurrencyCode),
                        new Currency(_settings.BaseCurrency),
                        rate.Rate / rate.Amount
                    ));

                return filteredRates;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred while fetching exchange rates: {ex.Message}");
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}