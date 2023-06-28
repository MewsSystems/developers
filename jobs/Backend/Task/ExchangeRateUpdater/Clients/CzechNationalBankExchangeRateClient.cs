using System.Net.Http;
using System.Text.Json;
using ExchangeRateUpdater.Clients.Models;

namespace ExchangeRateUpdater.Clients;

public class CzechNationalBankExchangeRateClient : ICzechNationalBankExchangeRateClient
{
    private readonly HttpClient _client;

    public CzechNationalBankExchangeRateClient(HttpClient client)
    {
        _client = client;
    }

    public CnbExchangeRates GetDailyRates()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "cnbapi/exrates/daily");
        using var response = _client.Send(request);
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStream();
        var exchangeRates = JsonSerializer.Deserialize<CnbExchangeRates>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return exchangeRates;
    }
}