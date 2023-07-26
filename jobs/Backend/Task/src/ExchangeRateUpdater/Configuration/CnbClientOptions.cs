namespace ExchangeRateUpdater.Features.Configuration
{
    public class ExchangeRateUpdaterOptions
    {
        public string BaseUrl { get; set; }
        public int RetriesNumber { get; set; }

        public string Timeout { get; set; }
    }
}
