using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;

namespace Adapter.ExchangeRateProvider.InMemory;

public class ExchangeRateProviderRepositoryInMemory : IExchangeRateProviderRepository
{
    private HashSet<ExchangeRate> _currencyRates;

    public ExchangeRateProviderRepositoryInMemory()
    {
        _currencyRates = new HashSet<ExchangeRate>();
    }

    public void AddExchangeRate(ExchangeRate exchangeRate)
    {
        _currencyRates.Add(exchangeRate);
    }

    public Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates()
    {
        return Task.FromResult(_currencyRates.AsEnumerable<ExchangeRate>());
    }
}
