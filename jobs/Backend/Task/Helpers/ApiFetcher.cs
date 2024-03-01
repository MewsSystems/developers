using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace ExchangeRateUpdater.Helpers;

public class ApiFetcher : IApiFetcher
{
    private const string ApiHostRoot = "https://api.cnb.cz/cnbapi";
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly Policy _policy;

    public ApiFetcher(ILogger logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();

        _policy = Policy.Handle<HttpRequestException>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        $"Request failed with {exception.Message}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                });
    }

    public ApiResponse GetExchangeRates()
    {
        _logger.LogInformation("Getting exchange rates from CNB API");
        var currentUtcTimestamp = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd");

        // get exchange rates from cb http with policy
        var result = _policy.Execute(async () => await GetApiResponse(currentUtcTimestamp));

        return result.Result;
    }

    private async Task<ApiResponse> GetApiResponse(string date)
    {
        // retrieve data from the API
        try
        {
            var response = await _httpClient.GetAsync($"{ApiHostRoot}/exrates/daily?date={date}&lang=EN");
            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync().Result;
            var responseJson = JsonConvert.DeserializeObject<ApiResponse>(responseString);
            return responseJson;
        }
        catch (JsonSerializationException ex)
        {
            _logger.LogError($"JSON deserialization failed {ex.Message}");
        }


        return null;
    }
}