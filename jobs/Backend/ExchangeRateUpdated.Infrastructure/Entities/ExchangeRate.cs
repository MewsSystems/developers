using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities
{
    public abstract class ExchangeRate
    {
        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal Value { get; }

        protected ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public override string ToString() => $"{SourceCurrency} / {TargetCurrency} = {Value}";
    }
}