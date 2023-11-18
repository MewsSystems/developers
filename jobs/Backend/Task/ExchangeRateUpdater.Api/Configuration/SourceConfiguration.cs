namespace ExchangeRateUpdater.Api.Configuration
{
    public class SourceConfiguration
    {
        public string BaseAddress { get; set; }
        public string DailyExchangeRatesEndpoint { get; set; }
        public string DefaultCurrency { get; set; }
    }
}
