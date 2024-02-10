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
            // The amount is not always 1 thus it should at least be stated and let the consumer decide
            // Other approach would be to divide it and normalize all rates to 1.
            return $"{SourceCurrency}/{TargetCurrency}={Value} (per {Amount})";
        }
    }
}