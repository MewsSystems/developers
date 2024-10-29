namespace ExchangeRateUpdater.Domain
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency,int amount, decimal value)
        {
            if (amount <= 0 ||
                value <= 0)
                throw new Exception($"Exchange {sourceCurrency.Code}/{targetCurrency.Code} value or amount incorrect.");

            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value / amount;
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
