using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class CNBRates
    {
        [JsonPropertyName("rates")]
        public IEnumerable<CNBRate> Rates { get; set; }
    }
}
