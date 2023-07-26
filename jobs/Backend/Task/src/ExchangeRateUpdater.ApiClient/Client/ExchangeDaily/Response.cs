namespace ExchangeRateUpdater.ApiClient.Client.ExchangeDaily
{
    public class ExchangeDailyResponse
    {
        public IEnumerable<ExchangeRateDailyResponse> Rates { get; set; }
    }

    public class ExchangeRateDailyResponse
    {
        public DateTime ValidFor { get; set; }
        public double Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }

    public class ErrorDailyResponse
    {
        public string Description { get; set; }
        public string EndPoint { get; set; }
        public string ErrorCode { get; set; }
        public DateTime HappenedAt { get; set; }
        public string MessageId { get; set; }
    }


}
