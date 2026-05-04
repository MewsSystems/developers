namespace ExchangeRateUpdater.Abstractions.Model
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    /// <param name="sourceCurrency"></param>
    /// <param name="targetCurrency"></param>
    /// <param name="value"></param>
    public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        /// <summary>
        /// The source currency of the exchange rate.
        /// </summary>
        public Currency SourceCurrency { get; } = sourceCurrency;

        /// <summary>
        /// The target currency of the exchange rate.
        /// </summary>
        public Currency TargetCurrency { get; } = targetCurrency;

        /// <summary>
        /// The value of the exchange rate, representing how much of the target currency one unit of the source
        /// currency is worth.
        /// </summary>
        public decimal Value { get; } = value;

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
