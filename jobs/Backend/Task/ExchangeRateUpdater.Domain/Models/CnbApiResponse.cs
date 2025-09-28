using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Root response object from Czech National Bank API
/// </summary>
public class CnbApiResponse
{
    [JsonPropertyName("rates")]
    public List<CnbExchangeRateDto> Rates { get; set; } = new();
}

public class CnbExchangeRateDto
{
    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; } = string.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}
