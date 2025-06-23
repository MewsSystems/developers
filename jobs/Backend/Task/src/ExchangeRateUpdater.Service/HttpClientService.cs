using ExchangeRateUpdater.Domain.Ack;
using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdater.Service
{
    public class HttpClientService : IHttpClientService
    {
        private readonly PollyConfig config;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HttpClientService> logger;

        private readonly Lazy<AsyncRetryPolicy<HttpResponseMessage>> retryPolicy;

        public HttpClientService(IHttpClientFactory httpClientFactory,
                                 IOptions<PollyConfig> config,
                                 ILogger<HttpClientService> logger)
        {
            this.config = config.Value;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            retryPolicy = new Lazy<AsyncRetryPolicy<HttpResponseMessage>>(CreateRetryPolicy);
        }

        /// <summary>
        /// Generic get http call, using Polly retry policies
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="httpClientName"></param>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AckEntity<TResult>> GetAsync<TResult>(string httpClientName, string uri)
        {
            if (string.IsNullOrEmpty(httpClientName) || string.IsNullOrEmpty(uri))
                return LogAndReturnOnError<TResult>("Missing data on API call");

            try
            {
                using HttpClient client = httpClientFactory.CreateClient(httpClientName); 

                var response = await retryPolicy.Value.ExecuteAsync(() => client.GetAsync(uri));

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                    return LogAndReturnOnError<TResult>(
                        $"API call to '{httpClientName}' '{uri}' failed with status {response.StatusCode}. Response: {responseContent}",
                        response.StatusCode);

                return new AckEntity<TResult>(true, JsonSerializer.Deserialize<TResult>(responseContent));
            }
            catch (Exception ex)
            {
                return LogAndReturnOnError<TResult>($"Unexpected error while calling {httpClientName}-{uri}: {ex.Message}");
            }
        }

        private AckEntity<TResult> LogAndReturnOnError<TResult>(string message, HttpStatusCode? code = null)
        {
            logger.LogError(message);
            return new AckEntity<TResult>(false, message, code);
        }

        private AsyncRetryPolicy<HttpResponseMessage> CreateRetryPolicy()
        {
            return Policy.Handle<HttpRequestException>()
                                .OrResult<HttpResponseMessage>(r => new[]
                                    {
                                        HttpStatusCode.RequestTimeout,
                                        HttpStatusCode.ServiceUnavailable,
                                        HttpStatusCode.GatewayTimeout
                                    }.Contains(r.StatusCode))
                                .WaitAndRetryAsync(config.RetryCountAttempts,
                                                   attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt) + config.SleepRetrySeconds),
                                                   (result, timeSpan, retryCount) =>
                                                   {
                                                       logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before retry {retryCount}.");
                                                   });
        }

    }
}

