using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;


namespace ExchangeRateUpdater.Infrastructure.Http;

/// <summary>
/// CNB-specific HTTP adapter. Replaces with ECBHttpRepository without Domain changes.
/// HTTP details encapsulated.
/// </summary>
public class CnbHttpExchangeRateRepository : IExchangeRateRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbHttpExchangeRateRepository> _logger;
    private const string CnbJsonRatesUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

    public CnbHttpExchangeRateRepository(HttpClient httpClient, ILogger<CnbHttpExchangeRateRepository> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mews-ExchangeRateUpdater/1.0");
    }

    public async Task<IReadOnlyDictionary<string, ExchangeRate>> GetCurrentRatesAsync(CancellationToken ct)
    {
        _logger.LogInformation("Fetching CNB JSON rates from {Url}", CnbJsonRatesUrl);
        
        var response = await _httpClient.GetAsync(CnbJsonRatesUrl, ct);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(ct);
        var rates = await ParseCnbJsonAsync(json);
        
        _logger.LogInformation("Parsed {Count} JSON rates from CNB API", rates.Count);
        return rates;
    }

    private async Task<Dictionary<string, ExchangeRate>> ParseCnbJsonAsync(string jsonContent)
    {
        var cnbResponse = JsonSerializer.Deserialize<CnbDailyResponse>(jsonContent);
        
        return cnbResponse.rates.ToDictionary(
            r => r.currencyCode,
            r => new ExchangeRate(r.currencyCode, r.rate, DateTime.UtcNow));
    }
}
