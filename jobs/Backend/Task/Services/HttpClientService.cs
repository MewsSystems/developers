using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private const int MAX_RETRIES = 3;
        private const int SECONDS_TO_RETRY = 2;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
                           .Handle<HttpRequestException>()
                           .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                           .WaitAndRetryAsync(MAX_RETRIES, retryAttempt => TimeSpan.FromSeconds(SECONDS_TO_RETRY),
                               (outcome, timespan, retryCount, context) =>
                               {
                                   if (outcome.Exception != null)
                                       Log.Warning("Retrying due to {Reason}. Retry attempt: {RetryCount}", outcome.Exception.Message, retryCount);
                                   else
                                       Log.Warning("Retrying due to unsuccessful status code {StatusCode}. Retry attempt: {RetryCount}", outcome.Result.StatusCode, retryCount);
                               });
        }

        public async Task<string> FetchDataAsync(string url)
        {
            Log.Debug("Sending request to {Url}", url);
            HttpResponseMessage response = null;
            try
            {
                response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(errorContent);
                    Log.Error("API call failed with error: {ErrorCode}, Description: {Description}", errorResponse.ErrorCode, errorResponse.Description);
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();
                Log.Debug("Received response: {Response}", content);
                return content;
            }
            catch (HttpRequestException e)
            {
                Log.Error("HTTP request failed after retries: {Exception}", e.Message);
                return null;
            }
        }
    }
}
