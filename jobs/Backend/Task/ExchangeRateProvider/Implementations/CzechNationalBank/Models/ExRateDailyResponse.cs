using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Implementations.CzechNationalBank.Models;

internal class ExRateDailyResponse
{
    [JsonPropertyName("rates")]
    public ExRateDailyRest[] Rates { get; set; } = null!;
}