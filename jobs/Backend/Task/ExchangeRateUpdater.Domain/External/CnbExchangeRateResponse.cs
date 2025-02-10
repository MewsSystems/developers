using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.Providers;

/// <summary>
/// Represents the response from the CNB exchange rate API.
/// </summary>
public sealed class CnbApiResponse
{
    [JsonPropertyName("rates")]
    public List<CnbExchangeRate> Rates { get; init; } = new();
}

/// <summary>
/// Represents a single exchange rate entry from CNB API.
/// </summary>
public sealed class CnbExchangeRate
{
    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }

    [JsonPropertyName("amount")]
    public int Amount { get; init; }
}
