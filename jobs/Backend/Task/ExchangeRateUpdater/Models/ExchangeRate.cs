using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            if (sourceCurrency == null)
            {
                throw new ArgumentNullException(nameof(sourceCurrency), "Source currency cannot be null.");
            }

            if (targetCurrency == null)
            {
                throw new ArgumentNullException(nameof(targetCurrency), "Target currency cannot be null.");
            }

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Exchange rate value must be greater than zero.");
            }

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
    }
}
