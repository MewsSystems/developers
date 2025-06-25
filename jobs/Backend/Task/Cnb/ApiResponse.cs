using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Cnb
{
    public class ApiResponse
    {
        [JsonPropertyName("rates")]
        public List<CnbRateDto> Rates { get; set; }
    }
}
