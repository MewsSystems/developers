namespace ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models
{
    public class ExchangeRate
    {
        public int Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public string ValidFor { get; set; }
    }

    public class ExchangeRateResponse
    {
        public IEnumerable<ExchangeRate> Rates { get; set; }
    }
}
