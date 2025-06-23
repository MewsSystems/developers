using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public class ExchangeRatesService : IExchangeRatesService
{
    private readonly IHttpClientFactory _httpClientFactory;


    public ExchangeRatesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ExchangeRatesResponseModel> GetExchangeRatesAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get exchange rates: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ExchangeRatesResponseModel>(content);
    }
}
