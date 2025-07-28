using System.Text.Json.Serialization;

namespace Mews.ExchangeRateUpdater.Infrastructure.Dtos;

public class CnbResponse
{
    [JsonPropertyName("rates")]
    public List<CnbRateDto> Rates { get; set; } = new();
}