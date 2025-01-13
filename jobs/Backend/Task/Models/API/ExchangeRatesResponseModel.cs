using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models.API;

public record ExchangeRatesResponseModel
{
    [JsonPropertyName("rates")]
    public List<ExchangeRateResponseModel> Rates { get; init; }
}