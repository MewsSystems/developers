using System;

namespace ExchangeRateUpdater
{
    public readonly struct Currency
    {
        public Currency(string code)
        {
            if (code == null || code.Length != 3) {
                throw new ArgumentException($"Invalid currency code '{code}'. Expected a three-letter code.");
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
