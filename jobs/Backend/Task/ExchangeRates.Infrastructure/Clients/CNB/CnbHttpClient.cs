using ExchangesRates.Infrastructure.External.CNB.Dtos;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExchangeRates.Infrastructure.Clients.CNB
{
    public interface ICnbHttpClient
    {
        Task<CnbExRatesResponse?> GetDailyExchangeRatesAsync(string? date = null, string lang = "EN", CancellationToken cancellationToken = default);
    }

    public class CnbHttpClient : ICnbHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CnbHttpClient> _logger;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CnbHttpClient(HttpClient httpClient, ILogger<CnbHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Gets the daily exchange rates from the Czech National Bank API.
        /// </summary>
        public async Task<CnbExRatesResponse?> GetDailyExchangeRatesAsync(string? date = null, string lang = "EN", CancellationToken cancellationToken = default)
        {
            var query = $"?lang={lang}";
            if (!string.IsNullOrEmpty(date))
                query += $"&date={date}";

            var endpoint = $"/cnbapi/exrates/daily{query}";

            _logger.LogInformation("Requesting CNB daily exchange rates from {Endpoint}", endpoint);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                using var response = await _httpClient.GetAsync(endpoint, cancellationToken);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("CNB API returned 404: Data not found for date {Date}.", date ?? "latest");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);
                    var message = $"CNB API returned error {(int)response.StatusCode}: {response.ReasonPhrase}. Body: {body}";
                    _logger.LogError(message);
                    throw new HttpRequestException(message);
                }

                var result = await response.Content.ReadFromJsonAsync<CnbExRatesResponse>(_jsonOptions, cancellationToken);

                if (result == null || result.Rates == null || !result.Rates.Any())
                {
                    _logger.LogWarning("CNB API returned an empty response for date {Date}.", date ?? "latest");
                    return null;
                }

                stopwatch.Stop();
                _logger.LogInformation("Successfully retrieved {Count} exchange rates from CNB in {ElapsedMilliseconds}ms.",
                    result.Rates.Count, stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching CNB exchange rates.");
                throw;
            }
        }
    }
}
