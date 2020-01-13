using System;

namespace ExchangeRateUpdaterV2.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
            TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public decimal RoundedValue => Math.Round(Value, 2);

        public override string ToString()
        {
            return $"{SourceCurrency} / {TargetCurrency} = {RoundedValue} {TargetCurrency}";
        }
    }
}
