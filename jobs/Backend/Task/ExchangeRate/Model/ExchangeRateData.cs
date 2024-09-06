using ExchangeRateUpdater.Extensions;

namespace ExchangeRateUpdater.ExchangeRate.Model
{
    /// <summary>
    /// Represents exchange rate data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExchangeRateData"/> class with the specified parameters.
    /// </remarks>
    /// <param name="currency">The currency.</param>
    /// <param name="currencyCode">The currency code.</param>
    /// <param name="country">The country.</param>
    /// <param name="rate">The exchange rate.</param>
    public class ExchangeRateData(string currency, string currencyCode, string country, decimal rate)
    {

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public string Currency { get; } = currency;

        /// <summary>
        /// Gets the currency code.
        /// </summary>
        public string CurrencyCode { get; } = currencyCode;

        /// <summary>
        /// Gets the country.
        /// </summary>
        public string Country { get; } = country;

        /// <summary>
        /// Gets the exchange rate.
        /// </summary>
        public decimal Rate { get; } = rate;

        /// <summary>
        /// Returns a string that represents the current exchange rate data.
        /// </summary>
        /// <returns>A string representation of the exchange rate data.</returns>
        public override string ToString()
        {
            return $"{Country.ToSentenceCase()} {Currency.ToSentenceCase()} ({CurrencyCode}) - {Rate}";
        }
    }
}
