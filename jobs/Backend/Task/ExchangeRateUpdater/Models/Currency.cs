using System;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        private static readonly Regex Iso4217Regex = new Regex("^[A-Z]{3}$", RegexOptions.Compiled);

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));
            }

            if (!Iso4217Regex.IsMatch(code))
            {
                throw new ArgumentException("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters.", nameof(code));
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
    }
}
