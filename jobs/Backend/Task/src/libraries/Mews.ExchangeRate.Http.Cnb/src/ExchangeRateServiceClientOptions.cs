using System;
using System.Collections.Generic;
using System.Text;

namespace Mews.ExchangeRate.Http.Cnb
{
    /// <summary>
    /// This class holds the settings to consume the CNB API.
    /// </summary>
    public class ExchangeRateServiceClientOptions
    {
        public const string ConfigurationSectionName = nameof(ExchangeRateServiceClientOptions);

        /// <summary>
        /// Gets or sets the API base URL.
        /// </summary>
        /// <value>
        /// The API base URL.
        /// </value>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the currency exchange rates endpoint.
        /// </summary>
        /// <value>
        /// The currency exchange rates endpoint.
        /// </value>
        public string CurrencyExchangeRatesEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the foreign exchange rates endpoint.
        /// </summary>
        /// <value>
        /// The foreign exchange rates endpoint.
        /// </value>
        public string ForeignExchangeRatesEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language { get; set; }
    }
}
