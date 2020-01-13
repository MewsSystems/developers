using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdaterV2.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));

            if (!IsValid)
            {
                throw new FormatException($"{code} is not ISO 4217 currency code");
            }
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        private bool IsValid => Code.Length == 3;
    }
}
