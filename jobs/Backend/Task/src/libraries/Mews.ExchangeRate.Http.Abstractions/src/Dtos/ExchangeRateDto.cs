using System;

namespace Mews.ExchangeRate.Http.Abstractions.Dtos
{
    /// <summary>
    /// This class is a Data Transfer Object that holds the properties of Exchange Rate.
    /// </summary>
    public class ExchangeRateDto
    {
        /// <summary>
        /// Gets or sets the amount that the `Rate` stands for.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the country Name.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the currency three-letter ISO 4217 code.
        /// </summary>
        /// <value>The currency code.</value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        /// <value>The currency.</value>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Gets or sets the sequence number of the rates published within the year.
        /// </summary>
        /// <value>The order.</value>
        public ushort Order { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate.
        /// </summary>
        /// <value>The rate.</value>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets the date which Exchange Rate Data is valid for.
        /// </summary>
        /// <value>The valid for.</value>
        public DateTime ValidFor { get; set; }
    }
}