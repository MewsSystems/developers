using System;

namespace Domain
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

        public override bool Equals(object obj)
        {
            return obj is Currency currency &&
                   Code == currency.Code;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code);
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
