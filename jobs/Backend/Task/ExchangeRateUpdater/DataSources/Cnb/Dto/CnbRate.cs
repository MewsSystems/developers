using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DataSource.Cnb.Dto;

internal sealed record CnbRate
{
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    [JsonPropertyName("country")]
    public string Country { get; init; }

    [JsonPropertyName("currency")]
    public string Currency { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }

    [JsonPropertyName("rate")]
    public Decimal Rate { get; init; }

    [JsonPropertyName("validFor")]
    public string ValidFor { get; init; }
}