using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Exchange_Providers.Provider.CNB
{
    /// <summary>
    /// Represents a response from the CNB (Czech National Bank) exchange rate provider containing exchange rate data.
    /// </summary>
    internal class CNB_Response
    {
        [JsonPropertyName("rates")]
        public List<CNB_Exchange_Rate> Rates { get; set; }
    }

    /// <summary>
    /// Represents an individual exchange rate from the CNB (Czech National Bank) exchange rate provider.
    /// </summary>
    internal class CNB_Exchange_Rate
    {
        /// <summary>
        /// Date for which the exchange rate is valid.
        /// </summary>
        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; set; }

        /// <summary>
        /// Order of the exchange rate.
        /// </summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }

        /// <summary>
        /// Country associated with the exchange rate.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// Currency name.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Amount of the currency.
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// ISO 4217 of currency code.
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Exchange rate value.
        /// </summary>
        [JsonPropertyName("rate")]
        public double Rate { get; set; }

    }
}
