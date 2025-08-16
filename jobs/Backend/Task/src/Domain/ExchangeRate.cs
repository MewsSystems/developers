using System;

namespace ExchangeRateUpdater.Domain
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            // This will be a good use case for Try.Aggregate
            // But, I've decided not to do so as it won't reflect how I'd do it currently

            SourceCurrency ??= sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
            TargetCurrency ??= targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));

            if (SourceCurrency.Code == TargetCurrency.Code)
                throw new ApplicationException($"Source and target currencies cannot refer to the same currency ({sourceCurrency.Code})");

            if (value < 0)
                throw new ApplicationException("Exchange rate value cannot be negative");

            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
