using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Net;

namespace ExchangeRateUpdater.Service
{
    public class HttpClientService : IHttpClientService
    {
        private readonly PollyConfig config;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HttpClientService> logger;

        private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy;


        public HttpClientService(IHttpClientFactory httpClientFactory,
                                 IOptions<PollyConfig> config,
                                 ILogger<HttpClientService> logger)
        {
            this.config = config.Value;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;

            retryPolicy = Policy.Handle<HttpRequestException>()
                                   .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.RequestTimeout)
                                   .WaitAndRetryAsync(this.config.RetryCountAttempts,
                                                      attempt => TimeSpan.FromSeconds(this.config.SleepRetrySeconds),
                                                      (result, timeSpan, retryCount) =>
                                                      {
                                                          logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before retry {retryCount}.");
                                                      });

            circuitBreakerPolicy = Policy
               .Handle<HttpRequestException>()
               .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
               .CircuitBreakerAsync(
                   this.config.ExceptionsAllowedBeforeBreaking,
                   TimeSpan.FromSeconds(this.config.DurationOfBreakSeconds),
                   onBreak: (result, breakDelay) =>
                   {
                       var logMessage = result.Exception != null
                           ? $"Circuit breaker opened due to exception: {result.Exception.Message}"
                           : $"Circuit breaker opened due to HTTP status: {result.Result.StatusCode}";

                       logger.LogWarning($"{logMessage}. Circuit will stay open for {breakDelay.TotalSeconds} seconds.");
                   },
                   onReset: () =>
                   {
                       logger.LogInformation("Circuit breaker reset. Requests are allowed again.");
                   },
                   onHalfOpen: () =>
                   {
                       logger.LogInformation("Circuit breaker is half-open, testing service health with a trial request.");
                   });
        }
    

        public async Task<TResult> GetAsync<TResult, TRequest>(string httpClientName, string uri, TRequest request)
        {
            if (request != null)
                uri = GetUriFromModelWithParams(uri, request);

            try
            {
                using HttpClient client = httpClientFactory.CreateClient(httpClientName); 

                var response = await retryPolicy.WrapAsync(circuitBreakerPolicy)
                                                .ExecuteAsync(() => client.GetAsync(uri));

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                    throw new ApiException($"API call to {httpClientName}-{uri} failed with status {response.StatusCode}. Response: {responseContent}");

                return JsonConvert.DeserializeObject<TResult>(responseContent);
            }
            catch (Exception e) when (e is HttpRequestException || e is ApiException)
            {
                logger.LogError(e.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error while calling {httpClientName}-{uri}: {ex.Message}");
                throw;
            }
        }

        private string GetUriFromModelWithParams<T>(string baseUrl, T model) =>
            $"{baseUrl}?{string.Join("&", model.GetType().GetProperties().Select(x => $"{x.Name}={x.GetValue(model)}"))}";
    }
}
