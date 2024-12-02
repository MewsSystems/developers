namespace ConsoleClient.Dtos
{
    public class ExchangeRateDto
    {
        public required CurrencyDto SourceCurrency { get; set; }
        public required CurrencyDto TargetCurrency { get; set; }
        public decimal Value { get; set; }
    }
}
