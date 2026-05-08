using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Responses
{
    public class ExchangeRateFixingResponse
    {
        [JsonPropertyName("rates")]
        public List<CurrencyRateModel> Rates { get; set; }
    }
}
