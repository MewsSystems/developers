using System.Text.Json.Serialization;

namespace Infrastructure.Models.CzechNationalBankModels;

public class CurrencyRateResponse
{
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}
