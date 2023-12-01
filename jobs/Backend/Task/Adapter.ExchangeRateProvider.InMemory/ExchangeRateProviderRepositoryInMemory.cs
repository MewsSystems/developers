using ExchangeRateUpdater.Domain.Ports;

namespace Adapter.ExchangeRateProvider.InMemory;

public class ExchangeRateProviderRepositoryInMemory : IExchangeRateProviderRepository
{
    public Task GetDefaultUnitRates()
    {
        return Task.CompletedTask;
    }
}
