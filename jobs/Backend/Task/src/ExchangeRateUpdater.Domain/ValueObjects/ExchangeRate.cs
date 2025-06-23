
namespace ExchangeRateUpdater.Domain.ValueObjects
{
    public class ExchangeRate : ValueObject
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

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return new object[] { SourceCurrency, TargetCurrency, Value };
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
