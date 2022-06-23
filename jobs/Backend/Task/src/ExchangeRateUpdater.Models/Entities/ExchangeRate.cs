namespace ExchangeRateUpdater.Models.Entities
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency), "Source currency should not be null.");
            TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency), "Target currency should not be null.");
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
