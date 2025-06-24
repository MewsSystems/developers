namespace ExchangeRateUpdater
{
    public class RateProviderConfiguration : IRateProviderConfiguration
    {
        public string Url { get; set; }
        public string BaseCurrency { get; set; }
    }
}
