using System.Collections.Generic;

namespace ExchangeRateUpdater
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

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ExchangeRate exchangeRate)) return false;
            if (ReferenceEquals(this, exchangeRate)) return true;
            return Value.Equals(exchangeRate.Value) && 
                   (SourceCurrency?.Code?.Equals(exchangeRate.SourceCurrency?.Code) ?? false) && 
                   (TargetCurrency?.Code?.Equals(exchangeRate.TargetCurrency?.Code) ?? false);
        }

        public override int GetHashCode()
        {
            return new {sourceCode=SourceCurrency?.Code, targetCode=TargetCurrency?.Code, Value}.GetHashCode();
        }

        public class SourceTargetEqualityComparer : IEqualityComparer<ExchangeRate>
        {
            public bool Equals(ExchangeRate x, ExchangeRate y)
            {
                if (x == null || y == null) return false;
                return (x.SourceCurrency?.Code?.Equals(y.SourceCurrency?.Code) ?? false) &&
                       (x.TargetCurrency?.Code?.Equals(y.TargetCurrency?.Code) ?? false);
            }

            public int GetHashCode(ExchangeRate obj)
            {
                return  new {sourceCode=obj.SourceCurrency?.Code, targetCode=obj.TargetCurrency?.Code}.GetHashCode();
            }
        }

    }
}
