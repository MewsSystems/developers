namespace ExchangeRateUpdaterConsole.Models
{
    public class ExchangeRateModel
    {
        public ExchangeRateModel(CurrencyModel sourceCurrency, CurrencyModel targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public CurrencyModel SourceCurrency { get; }

        public CurrencyModel TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
