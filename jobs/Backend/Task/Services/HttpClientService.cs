using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private const int MAX_RETRIES = 3;
        private const int SECONDS_TO_RETRY = 2;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
            _retryPolicy = Policy
                           .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                           .Or<HttpRequestException>()
                           .WaitAndRetryAsync(MAX_RETRIES, retryAttempt => TimeSpan.FromSeconds(SECONDS_TO_RETRY),
                               (outcome, timespan, retryCount, context) =>
                               {
                                   Log.Warning("Retrying due to {Reason}. Retry attempt: {RetryCount}",
                                       outcome.Result?.StatusCode.ToString() ?? outcome.Exception?.Message, retryCount);
                               });
        }

        public async Task<string> FetchDataAsync(string url)
        {
            Log.Debug("Sending request to {Url}", url);
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
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
    }
}