using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateApiResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; }

        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; }
    }
}
