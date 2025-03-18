using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IMemoryCache _cache;

    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger, TimeProvider timeProvider, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _timeProvider = timeProvider;
        _cache = cache;
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
            HttpResponseMessage response = await _httpClient.GetAsync($"cnapi/exrates/daily?date={_timeProvider.GetUtcNow():yyyy-MM-dd}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            ExchangeRateListDto exchangeRates = JsonSerializer.Deserialize<ExchangeRateListDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (exchangeRates != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Set cache expiration time

                _cache.Set(cacheKey, exchangeRates, cacheEntryOptions); // Set the cache here
                _logger.LogInformation("Exchange rates fetched from API and cached.");
                return exchangeRates;
            }
            else
            {
                _logger.LogError("Failed to deserialize exchange rate data.");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"HTTP request error: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError($"JSON deserialization error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"General error: {ex.Message}");
            return null;
        }

    }
}
