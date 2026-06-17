using System.Diagnostics;

namespace ExchangeRateUpdater
{
    [DebuggerDisplay("{SourceCurrency}/{TargetCurrency}={Value}")]
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            ArgumentNullException.ThrowIfNull(sourceCurrency);
            ArgumentNullException.ThrowIfNull(targetCurrency);

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
    }
}