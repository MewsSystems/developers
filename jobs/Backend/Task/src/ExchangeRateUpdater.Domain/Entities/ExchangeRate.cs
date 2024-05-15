using Ardalis.GuardClauses;
using System;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class ExchangeRate
    {
        public static ExchangeRate Create(Currency sourceCurrency, Currency targetCurrency, decimal value) =>
            new ExchangeRate(sourceCurrency, targetCurrency, value);

        private ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            Guard.Against.Null(sourceCurrency);
            Guard.Against.Null(targetCurrency);
            if (sourceCurrency.Equals(targetCurrency))
                throw new ArgumentException("Source currency and target currency must be not the same");

            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = Guard.Against.NegativeOrZero(value);
        }

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

    }
}
