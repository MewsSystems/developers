using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public static readonly Currency CZK = new Currency("CZK");

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
            if (obj is not Currency)
            {
                return false;
            }

            var other = obj as Currency;

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Code == other.Code;
        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }
    }
}
