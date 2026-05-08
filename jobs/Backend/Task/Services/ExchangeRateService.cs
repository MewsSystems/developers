using ExchangeRateUpdater.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService: IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly TimeProvider _timeProvider;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(HttpClient httpClient,
        TimeProvider timeProvider,
        IMemoryCache memoryCache,
        ILogger<ExchangeRateService> logger)
    {
        _httpClient = httpClient;
        _timeProvider = timeProvider;
        _cache = memoryCache;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ExchangeRatesDTO> GetExchangeRatesAsync()
    {
        if (_cache.TryGetValue("ExchangeRates", out ExchangeRatesDTO result))
        {
            _logger.LogInformation("Exchange rates retrieved from cache");
            return result;
        }

        var response = await _httpClient.GetAsync($"exrates/daily?date={_timeProvider.GetUtcNow():yyyy-MM-dd}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get exchange rates. Status code: {statusCode}", response.StatusCode);
            throw new ExchangeRateServiceException($"Failed to get exchange rates. Status code: {response.StatusCode}");
        }

        result = await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>();

        if (result.Rates.Any())
        {
            _cache.Set("ExchangeRates", result, TimeSpan.FromSeconds(60));
        }

        return result;
    }
}
