using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.ClientFactories
{
    public interface IExternalApiClientFactory
    {
        IExternalApiClient CreateApiClient();
    }
}
