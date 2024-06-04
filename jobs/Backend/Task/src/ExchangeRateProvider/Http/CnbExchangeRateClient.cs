using System.Text.Json;

namespace ExchangeRateProvider.Http;

public class CnbExchangeRateClient(HttpClient httpClient) : IExchangeRateClient
{
    public async Task<IEnumerable<CurrencyRate>> GetDailyExchangeRates()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "cnbapi/exrates/daily");
        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();

        var exRatesResponse = await JsonSerializer.DeserializeAsync<ExchangeRatesDailyResponse>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        if (exRatesResponse?.Rates is null) return [];

        return exRatesResponse.Rates
            .Where(r =>
                r.CurrencyCode != null &&
                r.Rate != null &&
                r.Amount != null);
    }
}