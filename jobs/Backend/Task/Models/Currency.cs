using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Currency currencyObject = (Currency)obj;

            return string.Equals(Code, currencyObject.Code, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var key = Code;
            return key.GetHashCode();
        }
    }
}
