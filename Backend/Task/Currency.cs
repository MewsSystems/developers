using System.Collections.Generic;

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
    }

    public class CurrencyEqualityComparer : IEqualityComparer<Currency>
    {
        public bool Equals(Currency x, Currency y) => y != null && (x != null && x.Code.Equals(y.Code));

        public int GetHashCode(Currency obj) => obj.GetHashCode();
    }
}
