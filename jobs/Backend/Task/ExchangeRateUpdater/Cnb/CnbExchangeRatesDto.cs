using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Cnb;

public class CnbExchangeRatesDto
{
    [JsonPropertyName("rates")]
    [Required]
    public IReadOnlyList<CnbExchangeRate> Rates { get; init; } = null!;
}

public class CnbExchangeRate
{
    [JsonPropertyName("validFor")]
    [Required]
    public DateTime ValidFor { get; init; }

    [JsonPropertyName("country")]
    [Required]
    public string CountryName { get; init; } = null!;

    [JsonPropertyName("currency")]
    [Required]
    public string CurrencyName { get; init; } = null!;

    [JsonPropertyName("amount")]
    [Required]
    public int Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    [Required]
    public string CurrencyCode { get; init; } = null!;

    [JsonPropertyName("rate")]
    [Required]
    public decimal ExchangeRate { get; init; }
}