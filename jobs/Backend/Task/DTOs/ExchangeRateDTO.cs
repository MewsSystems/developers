using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DTOs;

public record ExchangeRateDTO
{
    [JsonPropertyName("validFor")]
    public string ValidFor { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }

    [JsonPropertyName("currency")]
    public string Currency { get; init; }

    [JsonPropertyName("country")]
    public string Country { get; init; }

    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }
}
