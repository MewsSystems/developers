using ExchangeRateUpdater.Domain.Common;

namespace ExchangeRateUpdater.Domain.ValueObjects
{
    /// <summary>
    /// Represents a currency with a three-letter ISO 4217 code.
    /// </summary>
    public class Currency : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class with the specified code.
        /// </summary>
        /// <param name="code">The three-letter ISO 4217 code of the currency.</param>
        public Currency(string code)
        {
            Code = code.ToUpper();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Returns the three-letter ISO 4217 code of the currency as a string representation.
        /// </summary>
        public override string ToString()
        {
            return Code;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
