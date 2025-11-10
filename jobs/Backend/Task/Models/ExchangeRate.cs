using System;
using System.Runtime.ConstrainedExecution;

namespace ExchangeRateUpdater.Models
{
    /// <summary>
    /// Immutable value object representing an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        //validations in constructor
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
            TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));

            if (value <= 0)
                throw new ArgumentException("Exchange rate value must be positive.", nameof(value));

            Value = value;
        }

        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal Value { get; }

        //consistent output(4 decimal places)
        public override string ToString() =>
            $"{SourceCurrency}/{TargetCurrency}={Value:F4}";
    }
}