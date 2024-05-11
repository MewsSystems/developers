using System.Net.Http.Json;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;

internal sealed class ExchangeRateClient : IExchangeRateClient
{
    private readonly HttpClient _httpClient;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<ExchangeRateClient> _logger;

    public ExchangeRateClient(HttpClient httpClient, TimeProvider timeProvider, ILogger<ExchangeRateClient> logger)
    {
        _httpClient = httpClient;
        _timeProvider = timeProvider;
        _logger = logger;
    }
    
    public Task<IEnumerable<CnbRate>?> GetRates(CancellationToken cancellationToken)
    {
        return GetRates(_timeProvider.GetUtcNow().Date, cancellationToken);
    }

    public async Task<IEnumerable<CnbRate>?> GetRates(DateTime date, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"exrates/daily?lang=EN&date={date.Date:yyyy-MM-dd}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request to get exchange rates failed with status code {StatusCode} and response {Response}", response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken));
            return null;
        }
        
        var result = await response.Content.ReadFromJsonAsync<RateResponse>(cancellationToken);
        return result?.Rates;
    }
}