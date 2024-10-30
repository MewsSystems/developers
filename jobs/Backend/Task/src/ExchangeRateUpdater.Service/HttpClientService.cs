using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using System.Net;

namespace ExchangeRateUpdater.Service
{
    public class HttpClientService : IHttpClientService
    {
        private readonly PollyConfig config;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HttpClientService> logger;

        public HttpClientService(IHttpClientFactory httpClientFactory,
                                 IOptions<PollyConfig> config,
                                 ILogger<HttpClientService> logger)
        {
            this.config = config.Value;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<TResult> GetAsync<TResult, TRequest>(string httpClientName, string uri, TRequest request)
        {
            if (request != null)
                uri = GetUriFromModelWithParams(uri, request);

            var retryPolicy = Policy.Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.RequestTimeout)
                                    .WaitAndRetryAsync(config.RetryCountAttempts,
                                                       attempt => TimeSpan.FromSeconds(config.SleepRetrySeconds),
                                                       (result, timeSpan, retryCount) =>
                                                       {
                                                           logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before retry {retryCount}.");
                                                       });
            try
            {
                using HttpClient client = httpClientFactory.CreateClient(httpClientName);
                var response = await retryPolicy.ExecuteAsync(() => client.GetAsync(uri));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                    throw new ApiException($"API call to {httpClientName}-{uri} failed with status {response.StatusCode}. Response: {responseContent}", response.StatusCode);

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
