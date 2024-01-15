using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;

public record ExchangeRateApiModel
{
    [JsonPropertyName("currencyCode")]
    public required string CurrencyCode { get; init; }

    [JsonPropertyName("rate")]
    public required decimal Rate { get; init; }
    
    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }
}