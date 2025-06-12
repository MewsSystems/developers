using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Infrastructure
{
    /// <summary>
    /// An HTTP client wrapper that implements retry policies for resilient web requests.
    /// </summary>
    /// <remarks>
    /// This client:
    /// <list type="bullet">
    /// <item><description>Automatically retries failed requests with exponential backoff</description></item>
    /// <item><description>Handles transient HTTP errors and timeouts</description></item>
    /// <item><description>Configures default request headers for API communication</description></item>
    /// </list>
    /// </remarks>
    public class HttpWebClient : IHttpWebClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly SettingOptions _settingOptions;
        private readonly ILogger<HttpWebClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebClient"/> class.
        /// </summary>
        /// <param name="httpClient">The underlying HttpClient instance</param>
        /// <param name="settings">Configuration options for the client</param>
        /// <param name="logger">Logger instance for diagnostic information</param>
        public HttpWebClient(HttpClient httpClient, IOptions<SettingOptions> settings, ILogger<HttpWebClient> logger)
        {
            _settingOptions = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_settingOptions.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));

            // Retry policy 
            _retryPolicy = HttpPolicyExtensions
               .HandleTransientHttpError()
               .OrResult(msg => msg.StatusCode == HttpStatusCode.RequestTimeout)
               .WaitAndRetryAsync(5, retryAttempt =>
                   TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Executes a GET request to the configured endpoint with retry policy.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the <see cref="HttpResponseMessage"/> from the successful request.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync()
        {
            if (string.IsNullOrWhiteSpace(_settingOptions.Endpoint))
            {
                _logger.LogError("Endpoint configuration is missing or empty");
                throw new InvalidOperationException("Endpoint configuration is missing");
            }

            try
            {
                _logger.LogDebug("Initiating GET request to {Endpoint}", _settingOptions.Endpoint);

                var response = await _retryPolicy.ExecuteAsync(() =>
                    _httpClient.GetAsync(_settingOptions.Endpoint));

                response.EnsureSuccessStatusCode();

                _logger.LogDebug("Successfully received response from {Endpoint}", _settingOptions.Endpoint);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete GET request to {Endpoint} after retries", _settingOptions.Endpoint);
                throw;
            }
        }

    }
}
