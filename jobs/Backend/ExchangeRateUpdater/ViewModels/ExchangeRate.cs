using ExchangeRateUpdater.Utilities.Extensions;

namespace ExchangeRateUpdater.ViewModels
{
    class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, decimal value)
        {
            Guard.ArgumentNotNull(nameof(sourceCurrency), sourceCurrency);

            SourceCurrency = sourceCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency => Currencies.CZK;

        // TODO: Is formatting appropriate?

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
