using System;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Models
{
    public class RateDTO
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
        public string CurrencyCode { get; set; }
        
        [JsonProperty("rate")]
        public decimal RateValue { get; set; }
    }
}
