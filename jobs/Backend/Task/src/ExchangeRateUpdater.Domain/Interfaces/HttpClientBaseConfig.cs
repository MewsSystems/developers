namespace ExchangeRateUpdater.Domain.Interfaces
{
    public abstract class HttpClientBaseConfig
    {
        public string BaseUrl { get; set; }
        public string ClientName { get; set; }
        public int TimeOut { get; set; }
    }
}
