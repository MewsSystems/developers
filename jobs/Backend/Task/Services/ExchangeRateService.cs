using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;


namespace ExchangeRateUpdater.Services;

public class ExchangeRateService : IExchangeRateService
{
    /// <summary>
    ///  This service is  responsible for fetching exchange rate data from an api.cnb.cz/cnbapi/ API.
    ///It also implements resilience strategies using Polly for retries and caching mechanisms to reduce API calls.
    /// </summary>

    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IMemoryCache _cache;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;


    private static readonly ActivitySource ActivitySource = new("ExchangeRateService");

    public ExchangeRateService(IHttpClientFactory httpClientFactory, ILogger<ExchangeRateService> logger, TimeProvider timeProvider, IMemoryCache cache)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ExchangeRateService));
        _logger = logger;
        _timeProvider = timeProvider;
        _cache = cache;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                (outcome, timeSpan, retryCount, context) =>
                {
                    using var retryActivity = ActivitySource.StartActivity("PollyRetry");
                    retryActivity?.SetTag("retry.count", retryCount);
                    retryActivity?.SetTag("retry.waitTime", timeSpan.TotalSeconds);
                    retryActivity?.SetTag("retry.exception", outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString());

                    _logger.LogWarning($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });
    }

    public async Task<ExchangeRateListDto> GetExchangeRateListAsync()
    {
        var cacheKey = $"ExchangeRates_{_timeProvider.GetUtcNow():yyyy-MM-dd}";

        if (_cache.TryGetValue(cacheKey, out ExchangeRateListDto cachedRates))
        {
            _logger.LogInformation("Exchange rates retrieved from cache.");
            return cachedRates;
        }

        try
        {
            using var activity = ActivitySource.StartActivity("GetExchangeRates");

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync($"exrates/daily?date={_timeProvider.GetUtcNow():yyyy-MM-dd}")
            );

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            ExchangeRateListDto exchangeRates = JsonSerializer.Deserialize<ExchangeRateListDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (exchangeRates != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(cacheKey, exchangeRates, cacheEntryOptions);
                _logger.LogInformation("Exchange rates fetched from API and cached.");
                return exchangeRates;
            }
            else
            {
                _logger.LogError("Failed to deserialize exchange rate data.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"General error: {ex.Message}");
            throw;
        }
    }
}
