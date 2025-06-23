using System.Net.Http.Json;
using CurrencyExchange.Model;

namespace CurrencyExchange.Clients;

public class CnbApiClient(HttpClient httpClient) : ICurrencyExchangeClient
{
    private readonly HttpClient _httpClient = httpClient;

    /// <inheritdoc/>
    public async Task<DailyRatesResponse> GetDailyRates(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("cnbapi/exrates/daily", cancellationToken);
        response.EnsureSuccessStatusCode();

        try
        {
            return await response.Content.ReadFromJsonAsync<DailyRatesResponse>(cancellationToken) 
                ?? throw new Exception("CNB API returned unexpected null response.");
        }
        catch (Exception e)
        {
            throw new Exception("Error occurred during parsing a successful response from CNB API.", e);
        }
    }
}