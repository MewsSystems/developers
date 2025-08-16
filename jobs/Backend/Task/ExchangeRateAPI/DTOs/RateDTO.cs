namespace ExchangeRateUpdater.ExchangeRateAPI.DTOs
{
    public class RateDTO
    {
        public int Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public string ValidFor { get; set; }
    }
}
