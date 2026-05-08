using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Configuration;

namespace ExchangeRateUpdater.HttpClients
{
    public class CzechApiClient : ICzechApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly ILogger<CzechApiClient> _logger;

        public CzechApiClient(HttpClient httpClient, IOptions<CzechApiSettings> options, ILogger<CzechApiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseApiUrl = options?.Value?.BaseUrl ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAsync(string relativeUrl)
        {
            var url = $"{_baseApiUrl}{relativeUrl}";
            _logger.LogInformation("Sending GET request to {Url}", url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Received successful response from {Url}", url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}