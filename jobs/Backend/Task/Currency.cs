using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            Code = Normalize(code);
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        /// <summary>
        /// Normalized the given code convertinch it to upper case.
        /// </summary>
        private static string Normalize(string code)
        {
            return code.ToUpper();
        }
    }
}
