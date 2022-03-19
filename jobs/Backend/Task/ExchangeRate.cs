namespace ExchangeRateUpdater
{
    /// <summary>
    ///   Exchange Rate
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>Initializes a new instance of the <see cref="ExchangeRate" /> class.</summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <param name="targetCurrency">The target currency.</param>
        /// <param name="value">The value.</param>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>Initializes a new instance of the <see cref="ExchangeRate" /> class.</summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <param name="targetCurrency">The target currency.</param>
        /// <param name="value">The value.</param>
        public ExchangeRate(string sourceCurrency, string targetCurrency, decimal value)
        {
            SourceCurrency = new Currency(sourceCurrency);
            TargetCurrency = new Currency(targetCurrency);
            Value = value;
        }

        /// <summary>Gets the source currency.</summary>
        /// <value>The source currency.</value>
        public Currency SourceCurrency { get; }

        /// <summary>Gets the target currency.</summary>
        /// <value>The target currency.</value>
        public Currency TargetCurrency { get; }

        /// <summary>Gets the value. 1 unit of SourceCurrency = Amount/Value of TargetCurrency</summary>
        /// <value>The value.</value>
        public decimal Value { get; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
