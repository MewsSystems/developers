using System.Text.Json;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider(HttpClient httpClient): IExchangeRateProvider
{
    private const string TargetCurrencyCode = "CZK";

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
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
                r.Amount != null)
            .Select(r =>
                new ExchangeRate(
                    new Currency(r.CurrencyCode!),
                    new Currency(TargetCurrencyCode),
					decimal.Divide(r.Rate!.Value, r.Amount!.Value)));
	}
}