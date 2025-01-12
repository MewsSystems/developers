using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DTOs;

public record ExchangeRateDTO
{
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }
}
