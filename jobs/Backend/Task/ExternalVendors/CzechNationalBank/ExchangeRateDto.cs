namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;

    public partial class ExchangeRateDto
    {
        [JsonPropertyName("rates")]
        public Rate[] Rates { get; set; }
    }

    public partial class Rate
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
        public decimal RateRate { get; set; }
    }
}