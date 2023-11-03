using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Cnb;

public class CnbExchangeRatesDto
{
    [JsonPropertyName("rates")]
    [Required]
    public required IReadOnlyCollection<CurrencyRate> Rates { get; init; } = null!;
}

public class CurrencyRate
{
    [JsonPropertyName("validFor")]
    [Required]
    public required DateTime ValidFor { get; init; }

    [JsonPropertyName("country")]
    [Required]
    public required string CountryName { get; init; } = null!;

    [JsonPropertyName("currency")]
    [Required]
    public required string CurrencyName { get; init; } = null!;
    
    [JsonPropertyName("amount")]
    [Required]
    public required int Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    [Required]
    public required string CurrencyCode { get; init; } = null!;
    
    [JsonPropertyName("rate")]
    [Required]
    public required decimal ExchangeRate { get; init; }
}