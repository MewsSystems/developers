using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastucture;

public class CzRatesResponse
{
    [JsonPropertyName("rates")] public List<CzRate> Rates { get; set; }
}