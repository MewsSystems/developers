namespace ExchangeRateUpdater.Abstractions.Model
{
    /// <summary>
    /// Represents a currency with its ISO 4217 code.
    /// </summary>
    /// <param name="code"></param>
    public class Currency(string code)
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; } = code;

        /// <summary>
        /// Returns the ISO 4217 code of the currency.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Code;
        
        public bool Equals(Currency? other)
            => other != null && Code == other.Code;

        public override bool Equals(object? obj)
            => obj is Currency other && Equals(other);

        public override int GetHashCode()
            => Code.GetHashCode();
    }
}
