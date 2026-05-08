using System;
using System.Linq;

namespace ExchangeRateUpdater
{
    internal sealed class Currency
    {
        internal static readonly Currency Czk = new Currency("CZK");

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code),
                    "code is null or empty. Provide a valid ISO 4217 currency code.");
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("code must be exactly 3 characters.", nameof(code));
            }

            if (!code.All(x => char.IsUpper(x) && char.IsLetter(x)))
            {
                throw new ArgumentException("code must be consist of all upper characters", nameof(code));
            }
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;
            if (obj is not Currency other) return false;

            return Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
