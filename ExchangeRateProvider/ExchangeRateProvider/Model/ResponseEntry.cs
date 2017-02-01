using Newtonsoft.Json;

namespace ExchangeRateProvider.Model
{
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