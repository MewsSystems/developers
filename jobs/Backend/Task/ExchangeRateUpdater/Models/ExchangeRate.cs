using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRate : IExchangeRate
    {
        public ExchangeRate(ICurrency sourceCurrency, ICurrency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public ICurrency SourceCurrency { get; }

        public ICurrency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
