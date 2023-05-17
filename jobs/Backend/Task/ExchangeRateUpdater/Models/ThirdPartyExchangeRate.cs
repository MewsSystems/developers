namespace ExchangeRateUpdater.Models
{
    public class ThirdPartyExchangeRate
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
