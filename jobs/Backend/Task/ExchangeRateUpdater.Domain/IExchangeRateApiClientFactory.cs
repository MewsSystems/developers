namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClientFactory
    {
        IExchangeRateApiClient CreateExchangeRateApiClient(string currencyCode);
    }
}
