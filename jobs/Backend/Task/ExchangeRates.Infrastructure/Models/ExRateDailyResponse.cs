using System.Text.Json.Serialization;

namespace ExchangeRates.Infrastructure.Models;

public class ExRateDailyResponse
{
    [JsonPropertyName("rates")]
    public List<ExRateDailyRest> ExRates { get; set; }
}
