using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace ExchangeRateUpdater.Contract.ExchangeRate;

[UsedImplicitly]
public sealed record CnbExchangeRate(
    [property: JsonPropertyName("order")] int Order,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("currencyCode")] string CurrencyCode,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("rate")] decimal Rate
);

public sealed record CnbExchangeRatesResponse(
    [property: JsonPropertyName("rates")] IEnumerable<CnbExchangeRate> Rates
);