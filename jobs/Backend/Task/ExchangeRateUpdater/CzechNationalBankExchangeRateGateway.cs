using System.Net.Http;
using System.Text.Json;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRateGateway : ICzechNationalBankExchangeRateGateway
{
    private readonly HttpClient _client;

    public CzechNationalBankExchangeRateGateway(HttpClient client)
    {
        _client = client;
    }

    public CnbExchangeRates GetCurrentRates()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "cnbapi/exrates/daily?date=2023-06-27&lang=EN");
        using var response = _client.Send(request);
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStream();
        var exchangeRates = JsonSerializer.Deserialize<CnbExchangeRates>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return exchangeRates;
    }
}