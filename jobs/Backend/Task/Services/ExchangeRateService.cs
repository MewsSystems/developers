using ExchangeRateUpdater.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService: IExchangeRateService
{
    private IHttpClientFactory _httpClientFactory;

    public ExchangeRateService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<ExchangeRatesDTO> GetExchangeRates()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync("https://api.cnb.cz/cnbapi/exrates/daily?date=2025-11-01&lang=EN");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get exchange rates. Status code: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>();

        return result;
    }
}
