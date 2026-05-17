using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Application.Models;

public record RateResponse
{
    [JsonPropertyName("rates")]
    public IEnumerable<RateItem> Rates { get; set; } = default!;
}
