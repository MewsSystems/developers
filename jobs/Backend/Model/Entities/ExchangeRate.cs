using Model.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Model.Entities
{
    public class ExchangeRate : IEqualityComparer<ExchangeRate>
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
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public override int GetHashCode()
            => GetHashCode(this);

        public override bool Equals(object other)
            => other is Currency otherCurrency && Equals(otherCurrency);

        public bool Equals(ExchangeRate? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(this.SourceCurrency.Code, other.SourceCurrency.Code)
                && string.Equals(this.TargetCurrency.Code, other.TargetCurrency.Code)
                && decimal.Equals(this.Value, other.Value);
        }

        public bool Equals(ExchangeRate? x, ExchangeRate? y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode([DisallowNull] ExchangeRate obj)
        {
            return HashCode.Combine(obj.Value, obj.SourceCurrency, obj.TargetCurrency);
        }
    }
}
