using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeRateUpdater.CnbProvider.CnbClientResponses
{
    public class CnbRatesResponseDto
    {
        [JsonProperty("rates")]
        public IEnumerable<CnbRateResponseDto> Rates { get; set; }
    }
}
