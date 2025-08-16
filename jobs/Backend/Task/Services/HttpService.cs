using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace HttpApiService
{
    public class HttpService
    {
        private HttpClient httpClient = null;
        private AsyncRetryPolicy<HttpResponseMessage> httpRetryPolicy = null;
        private ILogger _logger = null;

        public HttpService(ILogger logger)
        {
            httpClient = new HttpClient();
            _logger = logger;
            httpRetryPolicy = Policy.Handle<HttpRequestException>().OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2), retryCount: 3));
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<T> GetWithJsonMapping<T>(string requestUrl)
        {
            try
            {
                HttpResponseMessage httpGetResponse = await httpRetryPolicy.ExecuteAsync(() => httpClient.GetAsync(requestUrl));

                if (httpGetResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Exchange rate API call successfull");
                    Stream httpGetResponseStream = await httpGetResponse.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<T>(httpGetResponseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"GET Request resulted in an exception: {ex.Message}");
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Deserializing GET Response resulted in an exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"HTTPService encountered an unhandled exception when calling {requestUrl}. {ex.Message}");
                throw;
            }

            return default;
        }
    }
}
