using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, DateTime date, int multiplier, decimal rate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Date = date.Date;
            Multiplier = multiplier;
            Rate = rate;
            //Value = CalculateValue(multiplier, rate);
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Rate { get; }

        public int Multiplier { get; }

        public DateTime Date { get; }

        public override bool Equals(object obj)
        {
            if (obj is not ExchangeRate)
            {
                return false;
            }

            var other = obj as ExchangeRate;

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.SourceCurrency.Equals(other.SourceCurrency) &&
                this.TargetCurrency.Equals(other.TargetCurrency) && 
                this.Date.Equals(other.Date);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.SourceCurrency, this.TargetCurrency, this.Date);
        }

        public override string ToString()
        {
            return $"{Multiplier} {SourceCurrency}/{TargetCurrency}={Rate}";
        }

        private decimal CalculateValue(int multiplier, decimal rate)
        {
            return rate / (decimal)Math.Pow(10, multiplier);
        }
    }
}
