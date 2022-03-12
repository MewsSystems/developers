using System;

namespace ExchangeRateProvider
{
    public class CurrencyCode
    {
        public CurrencyCode(string code)
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
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            CurrencyCode toCompare = obj as CurrencyCode;

            if (Object.ReferenceEquals(null, toCompare))
            {
                return false;
            }

            return this.Code == toCompare.Code;
        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }
    }
}
