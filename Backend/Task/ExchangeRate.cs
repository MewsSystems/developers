namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(int amount, Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            Amount = amount;
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }
        public int Amount { get; }
        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrency} = {Value} {TargetCurrency}";
        }
    }
}
