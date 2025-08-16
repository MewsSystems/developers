using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;

public class RateDto
{
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}