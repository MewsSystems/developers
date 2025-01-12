using ExchangeRateUpdater.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService: IExchangeRateService
{
    private HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
         _httpClient = httpClient;
    }        

    public async Task<ExchangeRatesDTO> GetExchangeRates()
    {
        var response = await _httpClient.GetAsync("exrates/daily?date=2025-11-01&lang=EN");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get exchange rates. Status code: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>();

        return result;
    }
}
