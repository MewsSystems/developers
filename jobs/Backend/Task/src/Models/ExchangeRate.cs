namespace ExchangeRateUpdater.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal amount, decimal rate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
            Amount = amount;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Amount { get; }

        public decimal Rate { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Amount}/{Rate}";
        }
    }
}
