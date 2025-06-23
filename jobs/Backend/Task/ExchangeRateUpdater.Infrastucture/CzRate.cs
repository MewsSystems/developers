using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastucture;

public class CzRate
{
    [JsonPropertyName("amount")] public long Amount { get; set; }

    [JsonPropertyName("country")] public string Country { get; set; }

    [JsonPropertyName("currency")] public string Currency { get; set; }

    [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }

    [JsonPropertyName("order")] public long Order { get; set; }

    [JsonPropertyName("rate")] public decimal Rate { get; set; }

    [JsonPropertyName("validFor")] public DateTimeOffset ValidFor { get; set; }
}