using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.Models.Response
{
    public class ExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRateResponse> Rates { get; set; }
    }

    public class ExchangeRateResponse
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
