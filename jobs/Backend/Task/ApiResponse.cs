using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class ApiResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<RateDto> Rates { get; set; }
    }
}
