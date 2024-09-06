namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents an exchange rate response item.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExchangeRateResponseItem"/> class.
    /// </remarks>
    /// <param name="currency">The name of the currency.</param>
    /// <param name="currencyCode">The currency code (e.g., USD).</param>
    /// <param name="country">The country associated with the currency.</param>
    /// <param name="rate">The exchange rate.</param>
    public class ExchangeRateResponseItem(string currency, string currencyCode, string country, decimal rate)
    {

        /// <summary>
        /// Gets the name of the currency.
        /// </summary>
        public string Currency { get; } = currency;

        /// <summary>
        /// Gets the currency code (e.g., USD).
        /// </summary>
        public string CurrencyCode { get; } = currencyCode;

        /// <summary>
        /// Gets the country associated with the currency.
        /// </summary>
        public string Country { get; } = country;

        /// <summary>
        /// Gets the exchange rate.
        /// </summary>
        public decimal Rate { get; } = rate;
    }
}
