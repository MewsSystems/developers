namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
		public ExchangeRate()
		{
		}

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; protected set; }

        public Currency TargetCurrency { get; protected set; }

        public decimal Value { get; protected set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
