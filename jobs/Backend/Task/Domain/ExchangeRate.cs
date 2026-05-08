namespace ExchangeRateUpdater.Domain
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> class.
        /// </summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <param name="targetCurrency">The target currency.</param>
        /// <param name="value">The exchange rate value.</param>
        /// <param name="validFor">The date the exchange rate is valid for.</param>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// The source currency.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// The target currency.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// The exchange rate value.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
