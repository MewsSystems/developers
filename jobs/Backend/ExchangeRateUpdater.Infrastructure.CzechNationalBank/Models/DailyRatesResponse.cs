using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;

public class DailyRatesResponse
{
    [JsonPropertyName("rates")]
    public List<RateDto> Rates { get; set; }
}