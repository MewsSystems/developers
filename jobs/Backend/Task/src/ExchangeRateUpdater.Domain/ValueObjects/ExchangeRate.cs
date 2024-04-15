namespace ExchangeRateUpdater.Domain.ValueObjects
{
    /// <summary>
    /// Represents a exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> class.
        /// </summary>
        /// <param name="sourceCurrency">The source currency of the exchange rate.</param>
        /// <param name="targetCurrency">The target currency of the exchange rate.</param>
        /// <param name="value">The value of the exchange rate from 1 unit of the source currency to the target currency.</param>
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// Source currency of the exchange rate.
        /// </summary>
        public Currency SourceCurrency { get; }

        /// <summary>
        /// Target currency of the exchange rate.
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// Value of the exchange rate from 1 unit of the source currency to the target currency.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Returns a string that represents the current exchange rate.
        /// </summary>
        /// <returns>A string that represents the current exchange rate: SourceCurrency/TargetCurrency=Value.</returns>
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
