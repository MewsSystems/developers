using System;

namespace ExchangeRateUpdater.CoreClasses
{
    public class Currency
    {
        public Currency(string code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException(nameof(code));

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

        public bool Equals(Currency curr)
        {
            if (curr == null)
                return false;
            return Code == curr.Code;
        }

        public override bool Equals(object obj)
        {
            if (obj is Currency)
                return Equals((Currency)obj);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
