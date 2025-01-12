using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models;

public record ExchangeRatesResponseModel
{
    [JsonPropertyName("rates")]
    public List<ExchangeRateResponseModel> Rates { get; init; }
}