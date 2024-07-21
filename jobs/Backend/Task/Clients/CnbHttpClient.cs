using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Clients;

public sealed class CnbHttpClient : ICnbHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbHttpClient> _logger;

    public CnbHttpClient(HttpClient httpClient, ILogger<CnbHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CnbExchangeRate>> GetDailyExchangeRates(CancellationToken cancellationToken)
    {
        var exchangeData = await _httpClient.GetFromJsonAsync<CnbExchangeData>("daily", cancellationToken);

        if (exchangeData is null)
        {
            _logger.LogWarning("Nothing was retrieved from CNB daily exchange rates api");
            return new List<CnbExchangeRate>();
        }
        
        _logger.LogInformation("Actual exchange rate was successfully retrieved from CNB");
        return exchangeData.Rates;
    }
}