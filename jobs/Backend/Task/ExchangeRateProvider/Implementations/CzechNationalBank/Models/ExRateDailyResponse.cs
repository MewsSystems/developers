using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Implementations.CzechNationalBank.Models;

public class ExRateDailyResponse
{
    [JsonPropertyName("rates")]
    public ExRateDailyRest[] Rates { get; set; } = null!;
}