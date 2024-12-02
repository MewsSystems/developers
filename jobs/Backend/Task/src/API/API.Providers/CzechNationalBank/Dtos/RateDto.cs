using Newtonsoft.Json;

namespace Provider.CzechNationalBank.Dtos
{
    internal class RateDto
    {
        [JsonProperty("validFor")]
        public required string ValidFor { get; set; }

        [JsonProperty("order")]
        public required int Order { get; set; }

        [JsonProperty("country")]
        public required string Country { get; set; }

        [JsonProperty("currency")]
        public required string Currency { get; set; }

        [JsonProperty("amount")]
        public required int Amount { get; set; }

        [JsonProperty("currencyCode")]
        public required string CurrencyCode { get; set; }

        [JsonProperty("rate")]
        public required decimal? Rate { get; set; }
    }
}
