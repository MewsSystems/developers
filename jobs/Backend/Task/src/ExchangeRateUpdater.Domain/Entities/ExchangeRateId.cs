using System;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class ExchangeRateId
    {
        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }

        protected bool Equals(ExchangeRateId other)
        {
            return SourceCurrency.Equals(other.SourceCurrency) && TargetCurrency.Equals(other.TargetCurrency);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExchangeRateId)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceCurrency, TargetCurrency);
        }

        
        public ExchangeRateId(Currency sourceCurrency, Currency targetCurrency)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
        }


    }
}