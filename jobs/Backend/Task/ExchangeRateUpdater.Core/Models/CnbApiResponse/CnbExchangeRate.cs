using System.Text.Json.Serialization;

namespace ExchangeRates.Core.Models.CnbApiResponse
{
    /// <summary>
    /// Class to describe exchange rates in the response from the CNB API.
    /// </summary>
    public class CnbExchangeRate
    {
        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = null!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; } = null!;

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}