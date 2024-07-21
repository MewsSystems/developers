using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    public partial class ExchangeRateDto
    {
        public static readonly ExchangeRateDto Empty = new ExchangeRateDto()
        {
            Rates = Array.Empty<ExchangeRateResult>()
        };

        [JsonProperty("rates", Required = Required.Always)]
        public ICollection<ExchangeRateResult> Rates { get; set; }
    }

    public partial class ExchangeRateResult
    {
        [JsonProperty("validFor", Required = Required.Always)]
        public DateTimeOffset ValidFor { get; set; }

        [JsonProperty("order", Required = Required.Always)]
        public long Order { get; set; }

        [JsonProperty("country", Required = Required.Always)]
        public string Country { get; set; }

        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public long Amount { get; set; }

        [JsonProperty("currencyCode", Required = Required.Always)]
        public string CurrencyCode { get; set; }

        [JsonProperty("rate", Required = Required.Always)]
        public decimal Rate { get; set; }
    }
}