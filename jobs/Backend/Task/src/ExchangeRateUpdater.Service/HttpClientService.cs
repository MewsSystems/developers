using ExchangeRateUpdater.Domain.Config;
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
        private readonly ILogger<HttpClientService> logger;

        public HttpClientService(IOptions<PollyConfig> config, ILogger<HttpClientService> logger)
        {
            this.config = config.Value;
            this.logger = logger;
        }

        public async Task<TResult> GetAsync<TResult, TRequest>(string uri, TRequest request)
        {
            if (request != null)
                uri = GetUriFromModelWithParams(uri, request);

            try
            {
                var retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.RequestTimeout)
                        .WaitAndRetryAsync(config.RetryCountAttempts, i => TimeSpan.FromSeconds(config.SleepRetrySeconds));

                using var client = new HttpClient();

                var result = await retryPolicy.ExecuteAsync(async () => await client.GetAsync(uri));
                var response = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                    throw new Exception($"The http call to: {uri} return a {result.StatusCode} code. Message: {response}. {DateTime.Now}");

                return JsonConvert.DeserializeObject<TResult>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        private string GetUriFromModelWithParams<T>(string baseUrl, T model) =>
            $"{baseUrl}?{string.Join("&", model.GetType().GetProperties().Select(x => $"{x.Name}={x.GetValue(model)}"))}";
    }
}
