namespace ExchangeRateUpdater.Domain
{
    /// <summary>
    /// Represents a currency with a three-letter ISO 4217 code.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class with the specified code.
        /// </summary>
        /// <param name="code">The three-letter ISO 4217 code of the currency.</param>
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Code;
        }
    }
}
