using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;
using ExchangeRateUpdater.ConfigurationOptions;
using ExchangeRateUpdater.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly TimeProvider _timeProvider;
    private readonly CnbClientOptions _cnbClientOptions;
    private readonly ILogger<ApiClient> _logger;
    private const string CacheContextOperationKeyTemplate = "daily-rates-{0}";

    public ApiClient(
        HttpClient httpClient,
        IOptions<CnbClientOptions> cnbClientOptions,
        ILogger<ApiClient> logger,
        TimeProvider timeProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _timeProvider = timeProvider;
        _cnbClientOptions = cnbClientOptions.Value;
    }

    public async Task<DailyExchangeRateApiModel?> GetDailyExchangeRatesAsync()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            new Uri(_httpClient.BaseAddress + _cnbClientOptions.DailyExchangeRatesPath));
        httpRequestMessage.SetPolicyExecutionContext(new Context(GetCacheContextOperationKey()));

        try
        {
            var response = await _httpClient.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DailyExchangeRateApiModel>(responseBody);
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException)
        {
            _logger.LogError(ex, "Exception during API request");
            throw new CnbApiClientException("Error during API request.", ex);
        }
    }

    private string GetCacheContextOperationKey() =>
        string.Format(CacheContextOperationKeyTemplate, _timeProvider.GetUtcNow().ToString("d", CultureInfo.InvariantCulture));
}