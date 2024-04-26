namespace ExchangeRateUpdater.Models
{
    public class ExchangeRate
    {
        public int Amount { get; }
        public Currency SourceCurrency { get; }
        public decimal Rate { get; }
        public Currency TargetCurrency { get; }

        public ExchangeRate(Currency targetCurrency, Currency sourceCurrency, decimal rate, int amount)
        {
            TargetCurrency = targetCurrency;
            SourceCurrency = sourceCurrency;         
            Rate = rate;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"In {Amount} {SourceCurrency} is {Rate} {TargetCurrency}";
        }
    }
}
