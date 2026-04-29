using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Business.Models
{
    public class ExchangeRateModel
    {
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
