using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeRateUpdater.CnbProvider.CnbClientResponses
{
    public class CnbRateResponseDto
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("validFor")]
        public string ValidFor { get; set; }
    }
}
