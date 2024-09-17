using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mews.ExchangeRate.Http.Cnb.Model
{
    /// <summary>
    /// This class holds the response DTO from the CNB API.
    /// </summary>
    public class ExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }

        public class ExchangeRate
        {
            /// <summary>
            /// Gets or sets the amount that the `Rate` stands for.
            /// </summary>
            /// <value>The amount.</value>
            [JsonPropertyName("amount")]
            public decimal Amount { get; set; }

            /// <summary>
            /// Gets or sets the country Name.
            /// </summary>
            /// <value>The country.</value>
            [JsonPropertyName("country")]
            public string Country { get; set; }

            /// <summary>
            /// Gets or sets the currency name.
            /// </summary>
            /// <value>The currency.</value>
            [JsonPropertyName("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// Gets or sets the currency ISO code.
            /// </summary>
            /// <value>The currency code.</value>
            [JsonPropertyName("currencyCode")]
            public string CurrencyCode { get; set; }

            /// <summary>
            /// Gets or sets the sequence number of the rates published within the year.
            /// </summary>
            /// <value>The order.</value>
            [JsonPropertyName("order")]
            public ushort Order { get; set; }

            /// <summary>
            /// Gets or sets the exchange rate.
            /// </summary>
            /// <value>The rate.</value>
            [JsonPropertyName("rate")]
            public decimal Rate { get; set; }

            /// <summary>
            /// Gets or sets the date which Exchange Rate Data is valid for.
            /// </summary>
            /// <value>The valid for.</value>
            [JsonPropertyName("validFor")]
            public string ValidFor { get; set; }
        }
    }
}