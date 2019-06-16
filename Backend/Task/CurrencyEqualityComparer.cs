using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class CurrencyEqualityComparer : IEqualityComparer<Currency>
    {
        public bool Equals(Currency x, Currency y) => y != null && (x != null && x.Code.Equals(y.Code));

        public int GetHashCode(Currency obj) => obj.GetHashCode();
    }
}