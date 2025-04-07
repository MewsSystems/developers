namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency SourceCurrency, decimal sourceAmount, decimal targetValue)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = "CZK";
            sourceAmount = sourceAmount;
            targetValue = targetValue;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal sourceAmount { get; }

        public decimal targetValue { get; }

        public override string ToString()
        {
            //handle Currency("XYZ") here?
            return $"{SourceCurrency}/{TargetCurrency}={sourceAmount}/{targetValue}";
        }
    }
}
