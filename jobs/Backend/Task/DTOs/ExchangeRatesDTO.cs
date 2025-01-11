using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.DTOs;

public record ExchangeRatesDTO
{
    [JsonPropertyName("rates")]
    public IEnumerable<ExchangeRateDTO> Rates { get; init; }
}
