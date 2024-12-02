using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Application.Models;

public record RateItem
{
    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; } = default!;

    [JsonPropertyName("currencyCode")]
    public string Currency { get; set; } = default!;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; } = default!;

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; } = default!;
}