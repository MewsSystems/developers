using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infra.Models;

public record ExchangeRateResponse
{
    public IEnumerable<ExchangeRate> Rates { get; init; } = Enumerable.Empty<ExchangeRate>();
   
}

public record ExchangeRate
{
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; } = "UNKNOWN";

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }
   
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
}