using System.Net.Http.Json;
using Exchange.Application.Abstractions.ApiClients;


namespace Exchange.Infrastructure.ApiClients;

public class CnbApiClient(HttpClient httpClient) : ICnbApiClient
{
    private const string ExchangeRatesUrl = "cnbapi/exrates/daily?lang=EN";

    public async Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(ExchangeRatesUrl, cancellationToken);
        var exchangeRates =
            await response.Content.ReadFromJsonAsync<ExchangeRatesResponse>(cancellationToken: cancellationToken);
        return exchangeRates?.Rates ?? [];
    }
}

file record ExchangeRatesResponse(List<CnbExchangeRate> Rates);