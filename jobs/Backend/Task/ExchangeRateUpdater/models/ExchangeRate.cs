namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, string amount, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Amount = amount;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }
        
        public string Amount { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{Amount} {SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
