using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class CnbRateDto
    {
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
