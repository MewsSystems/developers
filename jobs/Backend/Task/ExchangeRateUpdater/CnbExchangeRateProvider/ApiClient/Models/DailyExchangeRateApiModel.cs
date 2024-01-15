using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;

public record DailyExchangeRateApiModel
{
    [JsonPropertyName("rates")]
    public IList<ExchangeRateApiModel>? Rates { get; init; }
}