namespace ExchangeRateLayer.BLL.Objects
{
    public class ExchangeRate
    {
        public Currency DefaultCurrency { get; set; } = new Currency("CZK");
        public ExchangeRate(decimal value, Currency sourceCurrency, Currency targetCurrency = null)
        {
            targetCurrency = targetCurrency ?? DefaultCurrency;
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
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
