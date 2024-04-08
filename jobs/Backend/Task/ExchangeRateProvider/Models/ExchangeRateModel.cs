namespace ExchangeRateProvider.Models
{
    public class ExchangeRateModel
    {
        public CurrencyModel? SourceCurrency { get; set; }

        public CurrencyModel? TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}