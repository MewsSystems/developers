using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank;

public class CzechApiClient : IExchangeRateClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CzechApiClient> _logger;

    public CzechApiClient(IHttpClientFactory httpClientFactory, ILogger<CzechApiClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ExchangeRateApi");
        _logger = logger;
    }

    public async Task<ExchangeRateDto> GetDailyExchangeRates()
    {
        var uri = new Uri($"cnbapi/exrates/daily?date={DateTime.UtcNow:yyyy-MM-dd}&lang=EN", UriKind.Relative);
        var exchangeRates = await _httpClient.GetFromJsonAsync<ExchangeRateDto>(uri);

        return exchangeRates;
    }
}