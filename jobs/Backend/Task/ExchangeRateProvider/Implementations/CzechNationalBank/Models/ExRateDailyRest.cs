using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Implementations.CzechNationalBank.Models;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
internal class ExRateDailyRest
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = null!;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = null!;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = null!;

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("validFor")]
    public string ValidFor { get; set; } = null!;
}