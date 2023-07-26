namespace ExchangeRateUpdater.ApiClient.Client.ExchangeDaily
{
    public class ExchangeRequest
    {
        public DateTime DateTime { get; set; }
        public Language Language { get; set; }
    }

    public enum Language
    {
        CN, EN
    }
}
