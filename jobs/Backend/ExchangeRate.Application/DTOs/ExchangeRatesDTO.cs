namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRatesDTO
    {
        public CurrencyDTO? SourceCurrency { get; set; }

        public CurrencyDTO? TargetCurrency { get; set; }

        public DateTime Date { get; set; }
    }


}
