using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Interfaces;
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

    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger, TimeProvider timeProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<ExchangeRateListDto> GetExchangeRateListAsync()
    {

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"cnapi/exrates/daily?date={_timeProvider.GetUtcNow():yyyy-MM-dd}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            ExchangeRateListDto exchangeRates = JsonSerializer.Deserialize<ExchangeRateListDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (exchangeRates != null)
            {
                _logger.LogInformation("Exchange rates fetched from API.");
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
