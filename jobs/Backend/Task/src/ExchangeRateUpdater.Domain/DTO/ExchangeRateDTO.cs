using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.DTO
{
    public class ExchangeRateDTO
    {
        [JsonPropertyName("amount")]
        public int Amount { get; init; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; init; }

        [JsonPropertyName("currency")]
        public string Currency { get; init; }

        [JsonPropertyName("country")]
        public string Country { get; init; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; init; }

    }
}
