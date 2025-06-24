namespace ExchangeRateUpdater
{
    public interface IRateProviderConfiguration
    {
        string BaseCurrency { get; set; }
        string Url { get; set; }
    }
}