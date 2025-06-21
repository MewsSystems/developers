using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CnbApi.Models;

public class CnbExchangeRate
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; } = string.Empty;

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }

    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; } = string.Empty;
}