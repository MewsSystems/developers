using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class ExchangeRate: EntityBase<ExchangeRateId>
    {
        public override ExchangeRateId Id { get; }

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value): this(new ExchangeRateId(sourceCurrency, targetCurrency), value)
        {
        }

        public ExchangeRate(ExchangeRateId id, decimal value)
        {
            Id = id;
            Value = value;
        }

        public Currency SourceCurrency => Id.SourceCurrency;

        public Currency TargetCurrency => Id.TargetCurrency;

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}