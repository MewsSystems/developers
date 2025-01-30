using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models;

public class ExchangeRatesModel
{
    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }

    [JsonPropertyName("rate")]
    public double Rate { get; set; }

}
