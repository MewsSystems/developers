using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class Response
    {
        [JsonPropertyName("rates")]
        public List<RateDto> Rates { get; set; }
    }
}
