using System.Net.Http.Json;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.HttpClients;

public sealed class ExchangeRateApiClient : IExchangeRateApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateApiClient> _logger;

    public ExchangeRateApiClient(HttpClient httpClient, ILogger<ExchangeRateApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<CnbExchangeRate>> GetAllExchangeRatesAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CnbExchangeRateResponse>("exrates/daily", cancellationToken);
            if (response == null || response.Rates.Length == 0)
            {
                _logger.LogWarning("No exchange rates were provided by CNB");
                return [];
            }
            return response.Rates;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Http request to CNB has failed with exception {@exception}", ex);
            throw;
        }
    }
}