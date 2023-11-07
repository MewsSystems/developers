using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater
{
    [DebuggerDisplay("{Code}")]
    public class Currency
    {
        public Currency(string code)
        {
            ArgumentNullException.ThrowIfNull(code);
            if (code.Length != 3)
            {
                ThrowOnInvalidLength(nameof(code));
            }

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        [DoesNotReturn]
        private static void ThrowOnInvalidLength(string paramName) =>
            throw new ArgumentException("Currency code must be 3 characters long.", paramName);
    }
}