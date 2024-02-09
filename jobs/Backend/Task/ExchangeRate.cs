using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, long amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            Amount = amount;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public long Amount { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value} (per {Amount})";
        }
    }
}