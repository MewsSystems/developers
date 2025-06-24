using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class RateDto 
    {
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
