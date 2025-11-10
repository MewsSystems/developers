using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DataSource.Cnb.Dto;

internal sealed record CnbRateResponse
{
    [JsonPropertyName("rates")]
    public CnbRate[] Rates { get; init; }
}
