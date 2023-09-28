namespace ExchangeRateUpdater.Exchange_Providers.Models
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> class with source and target currencies and the exchange rate value.
        /// </summary>
        /// <param name="sourceCurrency">The source currency of the exchange rate.</param>
        /// <param name="targetCurrency">The target currency of the exchange rate.</param>
        /// <param name="value">The exchange rate value.</param>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// Gets the source currency of the exchange rate.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// Gets the target currency of the exchange rate.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// Gets the exchange rate value.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Returns a string representation of the exchange rate.
        /// </summary>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
