using System.Text.Json.Serialization;

namespace Adpater.Http.CzechNationalBank.Model
{
    public class GetDailyExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public DailyRate[] Rates { get; set; }
    }

    public class DailyRate
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("curremct")]
        public string Currency { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        [JsonPropertyName("validFor")]
        public string ValidFor { get; set;}
    }
}
