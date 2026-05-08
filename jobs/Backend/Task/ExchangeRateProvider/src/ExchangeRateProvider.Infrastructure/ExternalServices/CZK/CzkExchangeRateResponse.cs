using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

[ExcludeFromCodeCoverage(Justification = "External API response DTO with only JSON serialization attributes - no business logic")]
public record CzkExchangeRateResponse(
    List<CzkRate> Rates
);

[ExcludeFromCodeCoverage(Justification = "External API response DTO with only JSON serialization attributes - no business logic")]
public record CzkRate
{
    [JsonPropertyName("amount")]
    public required long Amount { get; init; }

    [JsonPropertyName("country")]
    public required string Country { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("currencyCode")]
    public required string CurrencyCode { get; init; }

    [JsonPropertyName("order")]
    public required int Order { get; init; }

    [JsonPropertyName("rate")]
    public required decimal Rate { get; init; }

    [JsonPropertyName("validFor")]
    public required DateOnly ValidFor { get; init; }
}
