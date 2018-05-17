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
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }

        protected bool Equals(ExchangeRate other) => Equals(SourceCurrency, other.SourceCurrency) && Equals(TargetCurrency, other.TargetCurrency);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ExchangeRate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceCurrency != null ? SourceCurrency.GetHashCode() : 0) * 397) ^ (TargetCurrency != null ? TargetCurrency.GetHashCode() : 0);
            }
        }
    }
}
