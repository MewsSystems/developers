using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;

public class CnbApiRatesResponse
{
    [JsonPropertyName("rates")]
    public List<RateDto>? Rates { get; set; }
}