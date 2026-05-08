using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Application.Services.Models;

internal record ExchangeRateProviderResponse(
[property: JsonPropertyName("rates")] IEnumerable<ExchangeRateProviderRate> Rates,
string ExchangeRateProviderCurrencyCode = "CZK");

internal record ExchangeRateProviderRate(
    [property: JsonPropertyName("amount")] int Amount,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("currencyCode")] string CurrencyCode,
    [property: JsonPropertyName("order")] int Order,
    [property: JsonPropertyName("rate")] decimal Rate,
    [property: JsonPropertyName("validFor")] string ValidFor);
