using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Retrieves normalized exchange rate data from the ÈNB API.
    /// The provider uses the configuration defined in CnbApiSettings.
    /// It returns rates only for the requested currencies (duplicates are filtered out)
    /// and ensures that if the base currency is requested yet missing, it is added with rate = 1.
    /// </summary>
    public class CnbApiExchangeRateDataProvider : IExchangeRateDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly CnbApiSettings _settings;

        public CnbApiExchangeRateDataProvider(HttpClient httpClient, CnbApiSettings settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IReadOnlyDictionary<string, (Currency Currency, decimal Rate)>> GetNormalizedRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies is null)
            {
                return new Dictionary<string, (Currency, decimal)>();
            }

            // Filter out duplicates (case-insensitive) by currency code.
            var currencyMap = currencies
                .GroupBy(c => c.Code.ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First());


            CnbApiResponse? response;
            try
            {
                // Fetch data from CNB API
                var endpoint = _settings.Endpoint.StartsWith("/")
                    ? _settings.Endpoint[1..]
                    : _settings.Endpoint;
                response = await _httpClient.GetFromJsonAsync<CnbApiResponse>(endpoint);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching exchange rates from ÈNB API", ex);
            }

            if (response?.Rates is null || !response.Rates.Any())
            {
                return new Dictionary<string, (Currency, decimal)>();
            }

            // Build mapping from currency code to normalized rate
            var normalizedRates = new Dictionary<string, (Currency, decimal)>(StringComparer.OrdinalIgnoreCase);

            // Map rates from API to requested currencies
            foreach (var (code, currency) in currencyMap)
            {
                // Find the matching rate from the API response
                var rateEntry = response.Rates.FirstOrDefault(r =>
                    string.Equals(r.CurrencyCode, code, StringComparison.OrdinalIgnoreCase));

                if (rateEntry is not null)
                {
                    // Calculate normalized rate as (rate divided by amount)
                    decimal normalizedRate = rateEntry.Rate / rateEntry.Amount;
                    normalizedRates[code.ToUpperInvariant()] = (currency, normalizedRate);
                }
            }

            // Ensure that if the requested currencies include the base currency (from configuration)
            // and it is missing from the result, we add an entry with a normalized rate of 1.
            if (currencyMap.Any(c => string.Equals(c.Key, _settings.BaseCurrency, StringComparison.OrdinalIgnoreCase))
                && !normalizedRates.ContainsKey(_settings.BaseCurrency.ToUpperInvariant()))
            {
                normalizedRates[_settings.BaseCurrency.ToUpperInvariant()] = (new Currency(_settings.BaseCurrency), 1m);
            }

            return normalizedRates;
        }
    }
}
