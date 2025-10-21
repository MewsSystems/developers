using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Services.Dtos
{
    public sealed record CnbApiResponseDto(
        [property: JsonPropertyName("rates")] IEnumerable<CnbApiRateDto> Rates
    );

    public sealed record CnbApiRateDto(
        [property: JsonPropertyName("currencyCode")] string CurrencyCode,
        [property: JsonPropertyName("amount")] int Amount,
        [property: JsonPropertyName("rate")] decimal Rate
    );
}