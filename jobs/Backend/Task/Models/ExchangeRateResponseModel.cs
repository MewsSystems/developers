using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models;

public record ExchangeRateResponseModel
{
    [JsonPropertyName("amount")]
    public long Amount { get; init; }
    
    [JsonPropertyName("currencyCode")]
    public required string CurrencyCode { get; init; }
    
    [JsonPropertyName("rate")]
    public required decimal Rate { get; init; }
    
    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; init; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; init; }
}