using System.Net.Http.Json;
using Exchange.Application.Abstractions.ApiClients;


namespace Exchange.Infrastructure.ApiClients;

public class CnbApiClient(HttpClient httpClient) : ICnbApiClient
{
    private const string ExchangeRatesUrl = "cnbapi/exrates/daily?lang=EN";

    public async Task<IEnumerable<ExchangeRateResponse>> GetExchangeRatesAsync()
    {
        var response = await httpClient.GetAsync(ExchangeRatesUrl);
        var exchangeRates = await response.Content.ReadFromJsonAsync<ExchangeRatesResponse>();
        return exchangeRates?.Rates ?? [];
    }
}

file record ExchangeRatesResponse(List<ExchangeRateResponse> Rates);