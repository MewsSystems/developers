using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CnbApi.Models;

public class CnbExchangeRatesResponse
{
    [JsonPropertyName("rates")]
    public List<CnbExchangeRate> Rates { get; init; } = [];
}