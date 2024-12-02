using ExchangeRates.Core.Models;
using ExchangeRates.Core.Models.CnbApiResponse;
using ExchangeRates.Core.Models.Configuration;
using ExchangeRates.Core.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRates.Core.Services
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateSettings _settings;
        private readonly ILogger<CnbExchangeRateProvider> _logger;
        private readonly IMemoryCache _cache;

        public CnbExchangeRateProvider(HttpClient httpClient,
            IOptions<ExchangeRateSettings> settings,
            ILogger<CnbExchangeRateProvider> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
            _cache = cache;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _logger.LogDebug("Getting exchange rates from CNB.");

            if (!currencies.Any())
            {
                // If no currencies are provided, return an empty list
                _logger.LogDebug("Requested currencies list is empty, returning an empty list.");
                return Enumerable.Empty<ExchangeRate>();
            }

            try
            {
                // Use caching to avoid fetching exchange rates from the CNB API too often.
                // This might not make sense in a console application that runs once, but given the exchange rates update only once a day
                // it would make sense to cache the exchange rates in a real-world application that receives multiple requests.
                // The cache duration is set in app configuration so it can be easily changed and is set to 1 minute for demonstration purposes.
                var cnbExchangeRates = _cache.GetOrCreate("CnbExchangeRates", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.CacheDurationInMinutes);

                    // Try to fetch exchange rates from the CNB API
                    _logger.LogInformation("Fetching exchange rates from CNB API.");
                    var response = _httpClient.GetAsync(_settings.CnbApiUrl).Result;
                    response.EnsureSuccessStatusCode();

                    // Deserialize the response content
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var exchangeRatesResponse = JsonSerializer.Deserialize<CnbExchangeRatesResponse>(responseContent);

                    if (exchangeRatesResponse == null || exchangeRatesResponse.Rates == null)
                    {
                        throw new InvalidOperationException("Failed to deserialize exchange rates response.");
                    }

                    return exchangeRatesResponse.Rates;
                });

                if (cnbExchangeRates == null)
                {
                    throw new InvalidOperationException("Failed to retrieve exchange rates from cache or CNB API.");
                }

                // Filter the exchange rates based on the provided currencies parameter and map
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));
                var filteredRates = cnbExchangeRates
                    .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
                    .Select(rate => new ExchangeRate(
                        new Currency(rate.CurrencyCode),
                        new Currency(_settings.BaseCurrency),
                        rate.Rate / rate.Amount
                    ));

                _logger.LogDebug("Successfully fetched and filtered exchange rates.");
                return filteredRates;
            }
            catch (Exception ex)
            {
                // TODO: Add project readme with documentation that exposes the source used
                _logger.LogError(ex, "An error occurred while fetching exchange rates.");
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}