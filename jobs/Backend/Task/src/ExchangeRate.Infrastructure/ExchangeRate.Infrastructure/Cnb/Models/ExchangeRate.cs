using System.Text.Json.Serialization;

namespace ExchangeRate.Infrastructure.Cnb.Models;

public class ExchangeRate
{
    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }

    [JsonPropertyName("rate")]
    public double Rate { get; set; }
}