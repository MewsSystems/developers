using System.Text.Json.Serialization;

namespace Infrastructure.Models.CzechNationalBankModels;

public class ExchangeRateDailyResponse
{
    [JsonPropertyName("rates")]
    public List<CurrencyRateResponse> Rates { get; set; } = new List<CurrencyRateResponse>();
}
