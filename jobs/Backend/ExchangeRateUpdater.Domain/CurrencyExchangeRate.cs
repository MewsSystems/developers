namespace ExchangeRateUpdater.Domain
{
    public class CurrencyExchangeRate
    {

        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public ExchangeRate Value { get; }


        public CurrencyExchangeRate(Currency sourceCurrency, Currency targetCurrency, ExchangeRate value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public override string ToString()
        {
            return $"{(string)SourceCurrency}/{(string)TargetCurrency}={(decimal)Value}";
        }
    }
}
