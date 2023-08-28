using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<ExchangeRateRecord> ExchangeRates { get; set; }
    }
}
