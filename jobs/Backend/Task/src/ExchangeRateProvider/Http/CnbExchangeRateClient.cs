using System.Text.Json;

namespace ExchangeRateProvider.Http;

public class CnbExchangeRateClient(HttpClient httpClient) : IExchangeRateClient
{
    public async Task<IEnumerable<CurrencyRate>> GetDailyExchangeRates(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "cnbapi/exrates/daily");
        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        var exRatesResponse = await JsonSerializer.DeserializeAsync<ExchangeRatesDailyResponse>(
	        stream,
	        new JsonSerializerOptions
	        {
		        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	        }).ConfigureAwait(false);

        if (exRatesResponse?.Rates is null) return [];

        return exRatesResponse.Rates
            .Where(r =>
                r.CurrencyCode != null &&
                r.Rate != null &&
                r.Amount != null);
    }
}