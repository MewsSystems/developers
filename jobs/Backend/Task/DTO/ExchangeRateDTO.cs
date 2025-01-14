using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DTOs;

public record ExchangeRateDTO
{
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }
}
