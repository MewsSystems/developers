using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateComparer : IEqualityComparer<ExchangeRate>
    {
        public bool Equals(ExchangeRate x, ExchangeRate y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.SourceCurrency.Code == y.SourceCurrency.Code
                && x.TargetCurrency.Code == y.TargetCurrency.Code
                && x.Rate == y.Rate;
        }

        public int GetHashCode(ExchangeRate obj)
        {
            return obj.GetHashCode();
        }
    }
}
