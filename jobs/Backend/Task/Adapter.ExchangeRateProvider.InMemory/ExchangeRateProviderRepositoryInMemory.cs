using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;

namespace Adapter.ExchangeRateProvider.InMemory;

public class ExchangeRateProviderRepositoryInMemory : IExchangeRateProviderRepository
{
    private Dictionary<ExchangeCurrencies, ExchangeRate> _currencyRates;

    public ExchangeRateProviderRepositoryInMemory()
    {
        _currencyRates = new Dictionary<ExchangeCurrencies, ExchangeRate>();
    }

    public void UpsertExchangeRate(ExchangeRate exchangeRate)
    {
        var key = new ExchangeCurrencies(exchangeRate.SourceCurrency, exchangeRate.TargetCurrency);

        if (_currencyRates.ContainsKey(key))
        {
            _currencyRates[key] = exchangeRate;
            return;   
        }

        _currencyRates.Add(key, exchangeRate);
    }

    public Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates()
    {
        return Task.FromResult(_currencyRates.Values.AsEnumerable<ExchangeRate>());
    }
}
