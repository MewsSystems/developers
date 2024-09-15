using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Implementations.CzechNationalBank.Models;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
internal class ExRateDailyResponse
{
    [JsonPropertyName("rates")]
    public ExRateDailyRest[] Rates { get; set; } = null!;
}