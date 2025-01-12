using ExchangeRateUpdater.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService
{
    private IHttpClientFactory _httpClientFactory;

    public ExchangeRateService()
    {
    }

    public ExchangeRateService(IHttpClientFactory httpClientFactory) 
        => _httpClientFactory = httpClientFactory;

    public async Task<ExchangeRatesDTO> GetExchangeRates()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var result = await httpClient.GetAsync("https://api.cnb.cz/cnbapi/exrates/daily?date=2019-05-17&lang=EN");

        return JsonSerializer.Deserialize<ExchangeRatesDTO>(result.Content.ReadAsStringAsync().Result);
    }
}
