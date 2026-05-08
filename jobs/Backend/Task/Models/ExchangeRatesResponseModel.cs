using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRatesResponseModel
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRatesModel> ExchangeRates { get; set; }
    }
}
