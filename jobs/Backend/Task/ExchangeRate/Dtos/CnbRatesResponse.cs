using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.ExchangeRate.Dtos
{
    public class CnbRatesResponse
    {
        [JsonPropertyName("rates")]
        public List<CnbRate> Rates { get; set; }
    }
}