using System.Text.Json.Serialization;

namespace Services.Models.CzechNationalBankApi;

public class CzkExchangeRateResponse
{
    [JsonPropertyName("rates")]
    public List<RateResponse> Rates { get; init; } = [];
}

public class RateResponse
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; } = string.Empty;

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }

    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; init; }
}

