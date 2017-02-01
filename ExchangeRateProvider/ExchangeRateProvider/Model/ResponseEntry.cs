using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateProvider.Model
{
    [JsonObject]
    public class ExchangeRatesRoot
    {
        [JsonProperty]
        public ExchangeRateEntry[] ExchangeRateEntries { get; set; }
    }

    [JsonObject]
    public class ExchangeRateEntry {

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty("TitleNo")]
        public string TitleNo { get; set; }

        [JsonProperty("TitleEn")]
        public string TitleEn { get; set; }

        [JsonProperty]
        public decimal CurrentValue { get; set; }
    }
}