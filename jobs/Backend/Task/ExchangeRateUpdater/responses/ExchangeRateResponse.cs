using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.responses
{


    public class ExchangeRateListResponse
    {
        [JsonProperty("rates")]
        public List<ExchangeRateResponse> Rates { get; set; }
    }
    public class ExchangeRateResponse
    {
        [JsonProperty("validFor")]
        public string ValidFor { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}