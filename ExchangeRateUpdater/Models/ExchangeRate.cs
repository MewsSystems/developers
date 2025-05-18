namespace ExchangeRateUpdater.Models
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> class with the specified source currency, target currency, and rate value.
        /// </summary>
        /// <param name="sourceCurrency">The source <see cref="Currency"/>.</param>
        /// <param name="targetCurrency">The target <see cref="Currency"/>.</param>
        /// <param name="value">The exchange rate value from source to target currency.</param>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// Source currency in the exchange rate.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// Traget currency in the exchange rate.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// Exchange rate value from the source to the target currency.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Returns a string representation of the exchange rate in the format "SOURCE/TARGET=VALUE".
        /// </summary>
        /// <returns>A string representation of the exchange rate.</returns>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
