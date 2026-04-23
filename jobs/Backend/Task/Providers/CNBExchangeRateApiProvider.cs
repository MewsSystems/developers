using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public class CNBExchangeRateApiProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CNBExchangeRateApiProvider> _logger;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        private readonly string _dailyRatesEndpoint;
        private readonly string _targetCurrencyCode;


        public CNBExchangeRateApiProvider(HttpClient httpClient, ILogger<CNBExchangeRateApiProvider> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;

            var section = configuration.GetSection("CNBExchangeRateAPIProvider");
            var apiBaseUrl = section.GetValue<string>("ApiBaseUrl") ?? throw new InvalidOperationException("ApiBaseUrl is not configured");
            _dailyRatesEndpoint = section.GetValue<string>("DailyRatesEndpoint") ?? throw new InvalidOperationException("DailyRatesEndpoint is not configured");
            _targetCurrencyCode = section.GetValue<string>("TargetCurrencyCode", "CZK")!;
            var timeoutSeconds = section.GetValue<int>("TimeoutSeconds", 30);
            var retryCount = section.GetValue<int>("RetryCount", 3);

            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            _retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount: retryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        var error = outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString() ?? "Unknown";
                        _logger.LogWarning("Retry {RetryCount} after {Delay}s due to: {Error}", retryCount, timespan.TotalSeconds, error);
                    });
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                _logger.LogWarning("No currencies provided");
                return [];
            }

            try
            {
                _logger.LogInformation("Fetching exchange rates from CNB API for {Count} currencies", currencies.Count());

                var response = await FetchDataAsync();
                var rates = ParseApiResponse(response, currencies);

                _logger.LogInformation("Successfully retrieved {Count} exchange rates from CNB API", rates.Count());
                return rates;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch exchange rates from CNB API");
                return [];
            }
        }

        private async Task<string> FetchDataAsync()
        {
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogDebug("Calling CNB API: {Endpoint}", _dailyRatesEndpoint);
                return await _httpClient.GetAsync(_dailyRatesEndpoint);
            });

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Received response from CNB API: {Length} characters", content.Length);
            return content;
        }

        private List<ExchangeRate> ParseApiResponse(string jsonContent, IEnumerable<Currency> requestedCurrencies)
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var apiResponse = JsonSerializer.Deserialize<CnbApiResponse>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Rates == null)
                {
                    _logger.LogWarning("Invalid API response: rates array is null or missing");
                    return rates;
                }

                _logger.LogDebug("Parsing {Count} rates from API response", apiResponse.Rates.Count);

                var requestedCodes = new HashSet<string>(
                    requestedCurrencies.Select(c => c.Code),
                    StringComparer.OrdinalIgnoreCase
                );

                rates = [.. apiResponse.Rates
                    .Where(r => !string.IsNullOrWhiteSpace(r.CurrencyCode))
                    .Where(r => !string.Equals(r.CurrencyCode, _targetCurrencyCode, StringComparison.OrdinalIgnoreCase))
                    .Where(r => r.CurrencyCode is not null && requestedCodes.Contains(r.CurrencyCode))
                    .Where(r => r.Amount != 0)
                    .Select(r =>
                    {
                        var normalizedRate = r.Rate / r.Amount;
                        _logger.LogDebug("Parsed rate: {Source}/{Target} = {Rate}", r.CurrencyCode, _targetCurrencyCode, normalizedRate);
                        return new ExchangeRate(new Currency(r.CurrencyCode!), new Currency(_targetCurrencyCode), normalizedRate);
                    })];

                return rates;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Failed to deserialize JSON response from CNB API");
                return rates;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unexpected error parsing API response");
                return rates;
            }
        }

        #region DTOs
        private class CnbApiResponse
        {
            public List<CnbRateData> Rates { get; set; } = new();
        }

        private class CnbRateData
        {
            [JsonPropertyName("currencyCode")]
            public string? CurrencyCode { get; set; }

            [JsonPropertyName("amount")]
            public int Amount { get; set; } = 1;

            [JsonPropertyName("rate")]
            public decimal Rate { get; set; }
        }
        #endregion
    }
}