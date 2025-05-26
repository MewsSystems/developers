using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.ExchangeRate.Dtos
{
    public class CnbRate
    {
        [JsonPropertyName("validFor")]
        public string ValidFor { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}