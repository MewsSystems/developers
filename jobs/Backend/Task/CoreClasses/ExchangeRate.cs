using System;

namespace ExchangeRateUpdater.CoreClasses
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, decimal sourceAmount, Currency targetCurrency, decimal targetAmount, decimal value)
        {
            if (sourceCurrency == null) throw new ArgumentNullException(nameof(sourceCurrency));
            if (targetCurrency == null) throw new ArgumentNullException(nameof(targetCurrency));
            if (targetCurrency.Equals(sourceCurrency)) throw new ArgumentException("Source and target currencies cannot be the same");

            SourceCurrency = sourceCurrency;
            SourceAmount = sourceAmount;
            TargetCurrency = targetCurrency;
            TargetAmount = targetAmount;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal SourceAmount { get; }

        public decimal TargetAmount { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceAmount} {SourceCurrency} / {TargetAmount} {TargetCurrency} = {Value}";
        }
    }
}
