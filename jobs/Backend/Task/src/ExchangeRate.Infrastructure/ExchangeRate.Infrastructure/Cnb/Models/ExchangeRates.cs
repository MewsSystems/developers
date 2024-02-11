using System.Text.Json.Serialization;

namespace ExchangeRate.Infrastructure.Cnb.Models;

public class ExchangeRates
{
    [JsonPropertyName("rates")]
    public List<ExchangeRate> Rates { get; set; } = null!;
}