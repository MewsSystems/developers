using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate : IEquatable<ExchangeRate>
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }

        public bool Equals(ExchangeRate other)
        {
            if (other == null)
                return false;
            return other.SourceCurrency.Code.Equals(SourceCurrency.Code)
                && other.TargetCurrency.Code.Equals(TargetCurrency.Code)
                && other.Value.Equals(Value);
        }
    }
}
