using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Models
{
    /// <summary>
    /// Represents a single exchange rate item retrieved from the CNB API response.
    /// </summary>
    public class CnbRateItem
    {
        /// <summary>
        /// The date when this exchange rate is applicable.
        /// </summary>
        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; set; }

        /// <summary>
        /// The order in which this currency appears in the CNB response.
        /// </summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }

        /// <summary>
        /// The name of the country associated with the currency.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// The descriptive name of the currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// The nominal amount of the currency for which the exchange rate is quoted.
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// The three-letter ISO 4217 currency code.
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// The exchange rate value in relation to CZK.
        /// </summary>
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
