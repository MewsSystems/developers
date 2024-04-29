using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain
{
    public class ExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<ExchangeRateData> Rates { get; set; }
    }

    public class ExchangeRateData
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
