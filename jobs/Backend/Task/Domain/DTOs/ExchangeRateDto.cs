using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.DTOs;

public class ExchangeRateDto
{
    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}