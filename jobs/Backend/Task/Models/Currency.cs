using System;

namespace ExchangeRateUpdater.Models
{
    /// <summary>
    /// Immutable value object representing a currency.
    /// </summary>
    public class Currency
    {
        //Validate in constructor. Don't allow invalid states.
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

            //Normalize to uppercase (usd : USD) for consistency
            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        // Equality based on Code to use it in HashSet/Dictionary
        public override bool Equals(object? obj) =>
            obj is Currency other && Code == other.Code;

        // Required when overriding Equals: Ensures Dictionary/HashSet work correctly
        public override int GetHashCode() => Code.GetHashCode();
    }
}