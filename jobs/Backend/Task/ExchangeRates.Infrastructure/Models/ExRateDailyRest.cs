using System.Text.Json.Serialization;

namespace ExchangeRates.Infrastructure.Models;

public class ExRateDailyRest
{
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}
