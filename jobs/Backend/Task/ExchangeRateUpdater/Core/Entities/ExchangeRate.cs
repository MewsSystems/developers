namespace ExchangeRateUpdater.Core.Entities
{
    public class ExchangeRate
    {
        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }


        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }


        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency} = {Value}";
        }
    }
}
