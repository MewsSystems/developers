using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangePair
    {
        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public ExchangePair(Currency sourceCurrency, Currency targetCurrency)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
        }

        public override bool Equals(object obj)
        {
            return obj is ExchangePair pair &&
                   SourceCurrency.Code == pair.SourceCurrency.Code &&
                   TargetCurrency.Code == pair.TargetCurrency.Code;
        }

        public override int GetHashCode()
        {
            int v = HashCode.Combine(SourceCurrency.Code, TargetCurrency.Code);
            return v;
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}";
        }
    }
}