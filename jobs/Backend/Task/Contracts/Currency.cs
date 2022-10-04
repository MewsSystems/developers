using System;
using System.Numerics;

namespace ExchangeRates.Contracts
{
    public class Currency: IEquatable<Currency>
    {
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

		public bool Equals(Currency other)
		{
			return this.Code.Equals(other.Code);
		}

		public override string ToString()
        {
            return Code;
        }

        public override bool Equals(object obj)
        {            
          return (obj is Currency) && this.Equals(obj as Currency);         
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
