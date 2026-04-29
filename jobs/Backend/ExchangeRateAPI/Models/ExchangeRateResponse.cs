using System.Text.Json.Serialization;

namespace ExchangeRateAPI.Models
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("sourceCurrency")]
        public string SourceCurrency { get; set; }

        [JsonPropertyName("targetCurrency")]
        public string TargetCurrency { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }
}
