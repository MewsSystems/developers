namespace ExchangeRateUpdater
{
    /// <summary>
    ///   Currency
    /// </summary>
    public class Currency
    {
        /// <summary>Initializes a new instance of the <see cref="Currency" /> class.</summary>
        /// <param name="code">The code.</param>
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Code;
        }
    }
}
