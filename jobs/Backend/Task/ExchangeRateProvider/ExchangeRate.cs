using System;

namespace ExchangeRateProvider
{
    public class ExchangeRate
    {
        public ExchangeRate(int amount, CurrencyCode sourceCurrency, CurrencyCode targetCurrency, decimal rate)
        {
            Amount = amount;
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
        }

        public int Amount { get; }

        public CurrencyCode SourceCurrency { get; }

        public CurrencyCode TargetCurrency { get; }

        public decimal Rate { get; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrency} = {Rate} {TargetCurrency}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            ExchangeRate toCompare = obj as ExchangeRate;

            if (Object.ReferenceEquals(null, toCompare))
            {
                return false;
            }

            return Amount == toCompare.Amount && SourceCurrency.Equals(toCompare.SourceCurrency)
                && TargetCurrency.Equals(toCompare.TargetCurrency) && Rate == toCompare.Rate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 29 + Amount.GetHashCode();
                hash = hash * 29 + SourceCurrency.GetHashCode();
                hash = hash * 29 + TargetCurrency.GetHashCode();
                hash = hash * 29 + Rate.GetHashCode();
                return hash;
            }
        }
    }
}
