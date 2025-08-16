using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Helpers;

public class ApiResponse
{
    public List<RateObject> Rates { get; set; }

    public class RateObject
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "validFor")]
        public string ValidFor { get; set; }

        [JsonProperty(PropertyName = "order")] 
        public string Order { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "currencyCode")]
        public string ISOCode { get; set; }

        [JsonProperty(PropertyName = "rate")] 
        public decimal Rate { get; set; }
    }
}