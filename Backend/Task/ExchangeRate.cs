namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal rate, decimal amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
            Amount = amount;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Rate { get; }

        public decimal Amount { get; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrency}/{TargetCurrency}={Rate}";
        }
    }
}
