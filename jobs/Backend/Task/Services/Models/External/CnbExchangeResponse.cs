using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Services.Models.External
{
    public sealed record CnbExchangeResponse(
        [property: JsonPropertyName("rates")] IEnumerable<CnbRate> Rates
    );

    public sealed record CnbRate(
        [property: JsonPropertyName("currencyCode")] string CurrencyCode,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("amount")] int Amount
    );
}