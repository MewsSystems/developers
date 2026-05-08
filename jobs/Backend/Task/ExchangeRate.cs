using System;

namespace ExchangeRateUpdater
{
    internal sealed class ExchangeRate
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

        /// <summary>
        ///     Returns Exchange Rate with CZK as a source currency
        /// </summary>
        /// <param name="targetCurrency"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ExchangeRate FromCZK(Currency targetCurrency, decimal value)
        {
            return new ExchangeRate(Currency.Czk, targetCurrency, value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;
            if (obj is not ExchangeRate other) return false;

            return Value == other.Value && SourceCurrency.Equals(other.SourceCurrency) &&
                   TargetCurrency.Equals(other.TargetCurrency);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, SourceCurrency, TargetCurrency);
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
