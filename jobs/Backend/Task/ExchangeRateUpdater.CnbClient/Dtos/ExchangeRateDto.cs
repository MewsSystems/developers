using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CnbClient.Dtos;

/// <summary>
/// Represents an exchange rate data transfer object (DTO).
/// </summary>
public class ExchangeRateDto
{
    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}
