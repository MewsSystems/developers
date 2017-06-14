namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, ExchangeRateType rateType, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            RateType = rateType;
            Value = value;
        }

        public ExchangeRateType RateType { get; private set; }

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }
    }
}
