namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderConfiguration : IExchangeRateProviderConfiguration
    {
        public string Url { get; set; }
        public string BaseCurrency { get; set; }
    }
}
