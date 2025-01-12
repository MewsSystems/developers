using ExchangeRateUpdater.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService: IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly TimeProvider _timeProvider;

    public ExchangeRateService(HttpClient httpClient, TimeProvider timeProvider)
    {
         _httpClient = httpClient;
        _timeProvider = timeProvider;
    }        

    public async Task<ExchangeRatesDTO> GetExchangeRates()
    {
        var todaysDate = _timeProvider.GetUtcNow().ToString("yyyy-MM-dd");
        var response = await _httpClient.GetAsync($"exrates/daily?date={todaysDate}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get exchange rates. Status code: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<ExchangeRatesDTO>();

        return result;
    }
}
