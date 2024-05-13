using ExchangeRate.Datalayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new InvalidCurrencyException();

            if (code.Length != 3)
                throw new InvalidCurrencyException("Currency must be in three-letter ISO 4217 code format");

            ThreeLetterISOCurrencyCode = code;
        }

        public string ThreeLetterISOCurrencyCode { get; }

        public override string ToString()
        {
            return ThreeLetterISOCurrencyCode;
        }
    }
}
