namespace ExchangeRateUpdater.Domain
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// Immutable and validated.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Source currency of the exchange rate.
        /// </summary>
        public Currency Source { get; }

        /// <summary>
        /// Target currency of the exchange rate.
        /// </summary>
        public Currency Target { get; }

        /// <summary>
        /// Exchange rate value (e.g., 1 USD = 23.5 CZK -> Value = 23.5).
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Creates a new ExchangeRate instance.
        /// </summary>
        /// <param name="source">Source currency (required).</param>
        /// <param name="target">Target currency (required).</param>
        /// <param name="value">Exchange rate value.</param>
        public ExchangeRate(Currency source, Currency target, decimal value)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Value = value;
        }
    }
}
