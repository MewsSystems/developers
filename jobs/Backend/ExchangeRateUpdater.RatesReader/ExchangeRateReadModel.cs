namespace ExchangeRateUpdater.RatesReader
{
    public class ExchangeRateReadModel
    {
        public string? CurrencyCode { get; set; }
        public decimal? Rate { get; set; }
        public int? Amount { get; set; }
    }
}
