using System;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Validate(Code);
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        private void Validate(string code)
        {
            if(!Regex.IsMatch(code, @"^[a-zA-Z]{3}$"))
            {
                throw new ArgumentException($"Code {code} isn't in ISO 4217 format.");
            }
        }
    }
}
