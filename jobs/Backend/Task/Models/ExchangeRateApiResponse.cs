using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRateApiResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<RateDTO> Rates { get; set; }
    }
}
