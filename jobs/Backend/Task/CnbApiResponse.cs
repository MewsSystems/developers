using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class CnbApiResponse
    {
        [JsonPropertyName("rates")]
        public List<CnbRateDto> Rates { get; set; }
    }
}
