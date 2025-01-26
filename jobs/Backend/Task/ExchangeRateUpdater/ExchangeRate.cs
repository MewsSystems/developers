namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal amount, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = value;
            Amount = amount;
        }

        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }

        public decimal Rate { get; }
        public decimal Amount { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Rate} (per {Amount})";
        }
    }
}
