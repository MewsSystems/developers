using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;

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

    public Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates(DateTime exhangerRateDate)
    {
        return Task.FromResult(_currencyRates.Values.AsEnumerable<ExchangeRate>());
    }

    public Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime From, DateTime To)
    {
        var key = new ExchangeCurrencies(sourceCurrency, targetCurrency);

        if (_currencyRates.ContainsKey(key))
        {
            return Task.FromResult<IEnumerable<ExchangeRate>>(new List<ExchangeRate> { _currencyRates[key] });
        }

        return Task.FromResult<IEnumerable<ExchangeRate>>(new List<ExchangeRate>());
    }
}
