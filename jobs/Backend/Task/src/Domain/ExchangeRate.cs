using System;
using System.Collections.Generic;

namespace Domain
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override bool Equals(object obj)
        {
            return obj is ExchangeRate rate &&
                   EqualityComparer<Currency>.Default.Equals(SourceCurrency, rate.SourceCurrency) &&
                   EqualityComparer<Currency>.Default.Equals(TargetCurrency, rate.TargetCurrency) &&
                   Value == rate.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceCurrency, TargetCurrency, Value);
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }


    }
}
