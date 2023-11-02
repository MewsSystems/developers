using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var now = DateTime.UtcNow;

        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.cnb.cz/cnbapi/exrates/daily?date={now:yyyy-MM-dd}&lang=EN");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        using var httpClient = new HttpClient();
        var response = httpClient.Send(request);

        var ratesPayload = response.Content.ReadFromJsonAsync<RatesResponsePayload>().GetAwaiter().GetResult();

        return ratesPayload.Rates
            .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
            .Select(
                r => new ExchangeRate(
                    sourceCurrency: new Currency(r.CurrencyCode),
                    targetCurrency: new Currency("CZK"),
                    value: r.ExchangeRate / r.Amount));
    }
}

public class RatesResponsePayload
{
    [JsonPropertyName("rates")]
    public IReadOnlyCollection<Rate> Rates { get; init; }
}

public class Rate
{
    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; init; }
    
    [JsonPropertyName("country")]
    public string CountryName { get; init; }
    
    [JsonPropertyName("currency")]
    public string CurrencyName { get; init; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
    
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }
    
    [JsonPropertyName("rate")]
    public decimal ExchangeRate { get; init; }
}
