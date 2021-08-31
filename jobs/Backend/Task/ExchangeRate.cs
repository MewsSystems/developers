namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, int amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            Amount = amount;
        }

        public int Amount { get; }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
