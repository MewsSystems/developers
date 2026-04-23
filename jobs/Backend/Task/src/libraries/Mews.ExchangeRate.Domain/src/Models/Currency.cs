using System;

namespace Mews.ExchangeRate.Domain.Models
{
    public class Currency
    {        
        public static readonly Currency CZK = new Currency("CZK");
        public static readonly Currency Default = CZK;

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
            return obj is Currency other
                && StringComparer.OrdinalIgnoreCase.Equals(Code, other.Code);
        }

        override public int GetHashCode()
        {
            return Code.ToUpperInvariant().GetHashCode();
        }
    }
}
