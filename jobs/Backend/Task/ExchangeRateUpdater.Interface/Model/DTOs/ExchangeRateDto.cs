namespace ExchangeRateUpdater.Interface.DTOs
{
    public class ExchangeRateDto
    {
        public CurrencyDto SourceCurrency { get; set; } = new CurrencyDto { Code = "CZK" };

        public CurrencyDto? TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
