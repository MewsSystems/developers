using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Common
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<CustomHttpClient> _logger;

        public CustomHttpClient(ILogger<CustomHttpClient> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends a GET request to the specified uri with a cancellation token as an asynchronous operation.
        /// Method is using retry policy.
        /// Result is serialized into specified object
        /// </summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <param name="url">Url of the request</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result from the specified url serialized into specified object</returns>
        public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            try
            {
                var response = await GetRetryPolicy().ExecuteAsync(() => _httpClient.GetAsync(url, cancellationToken));
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching data from {url}");
            }

            return default;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
