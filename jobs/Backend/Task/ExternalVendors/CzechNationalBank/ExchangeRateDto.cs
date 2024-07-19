using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    public partial class ExchangeRateDto
    {
        [JsonPropertyName("rates")]
        public ExchangeRateResult[] Rates { get; set; }
    }

    public partial class ExchangeRateResult
    {
        [JsonPropertyName("validFor")]
        public DateTimeOffset ValidFor { get; set; }

        [JsonPropertyName("order")]
        public long Order { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}