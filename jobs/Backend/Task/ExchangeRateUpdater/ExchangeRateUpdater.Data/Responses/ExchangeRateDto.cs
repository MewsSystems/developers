using Newtonsoft.Json;

namespace ExchangeRateUpdater.Data.Responses
{
    public class ExchangeRateDto
    {
        [JsonProperty("validFor")]
        public DateTime ValidFor { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string TargetCurrency { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}
