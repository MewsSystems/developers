using System.Net.Http;
using System.Text.Json;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRateGateway
{
    public CnbExchangeRates GetCurrentRates()
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://api.cnb.cz/cnbapi/exrates/daily?date=2023-06-27&lang=EN");
        using var response = httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStream();
        var exchangeRates = JsonSerializer.Deserialize<CnbExchangeRates>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return exchangeRates;
    }
}