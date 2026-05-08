using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Configuration;

namespace ExchangeRateUpdater.HttpClients
{
    // HTTP client implementation for CNB API.
    public class CnbHttpClient : ICnbHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly CnbApiSettings _settings;
        private readonly ILogger<CnbHttpClient> _logger;

        public CnbHttpClient(HttpClient httpClient, IOptions<CnbApiSettings> settings, ILogger<CnbHttpClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        }

        // Method's implementation: fetches exchange rate data from the CNB API and returns exchange rate data as a string in pipe-separated format
        public async Task<string> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_settings.BaseUrl);
                // Ensure the HTTP response is success
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogError("Received empty response from CNB API");
                    throw new InvalidOperationException("Received empty response from CNB API.");
                }

                return content;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request to CNB API failed: {Url}", _settings.BaseUrl);
                throw new InvalidOperationException($"Failed to fetch exchange rates from CNB API.", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request to CNB API timed out after {TimeoutSeconds} seconds", _settings.TimeoutSeconds);
                throw new TimeoutException($"Request to CNB API timed out after {_settings.TimeoutSeconds} seconds.", ex);
            }
        }
    }
}
