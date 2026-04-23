namespace ExchangeRateUpdater
{
    public interface IExchangeRateProviderConfiguration
    {
        string BaseCurrency { get; set; }
        string Url { get; set; }
    }
}