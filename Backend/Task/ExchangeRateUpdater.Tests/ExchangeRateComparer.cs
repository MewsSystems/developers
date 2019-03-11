using System.Collections.Generic;
using System;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateComparer : Comparer<ExchangeRate>
    {
        public override int Compare(ExchangeRate x, ExchangeRate y)
        {
           if (x.Value == y.Value)
            {
                if (x.SourceCurrency.Code.Equals(y.SourceCurrency.Code, StringComparison.Ordinal)
                    && x.TargetCurrency.Code.Equals(y.TargetCurrency.Code, StringComparison.Ordinal))
                {
                    return 0;
                }
                if (!x.SourceCurrency.Code.Equals(y.SourceCurrency.Code, StringComparison.Ordinal))
                {
                    return string.Compare(x.SourceCurrency.Code, y.SourceCurrency.Code, StringComparison.Ordinal);
                }
                return string.Compare(x.TargetCurrency.Code, y.TargetCurrency.Code, StringComparison.Ordinal);
            }
            return x.Value.CompareTo(y.Value);
        }
    }
}

