using System;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.Exceptions;

namespace ExchangeRateUpdater.Domain
{
    public record struct Currency
    {
        public Currency(string code)
        {
            if (code.Length != 3)
            {
                throw new NotValidCurrencyCodeException(code);
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