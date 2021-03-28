namespace ExchangeRateUpdater
{
    public class ExchangeRate
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

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public override bool Equals(object obj)
        {
            var otherRate = obj as ExchangeRate;

            return otherRate != null
                && otherRate.TargetCurrency.Equals(TargetCurrency)
                && otherRate.SourceCurrency.Equals(SourceCurrency)
                && otherRate.Value == Value;
        }
    }
}
