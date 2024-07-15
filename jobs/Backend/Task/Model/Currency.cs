using System;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Model
{
    public record Currency
    {

        private static readonly Regex ISO_4217 = new("^[A-Z]{3}$");
        
        public Currency(string code)
        {
            if (!ISO_4217.IsMatch(code)) throw new ArgumentException("Invalid currency code: " + code);
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
