using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace Adapter.ExchangeRateProvider.InMemory;

public class ExchangeRateProviderRepositoryInMemory : IExchangeRateProviderRepository
{
    private Dictionary<DateTime, ISet<ExchangeRate>> _currencyRates;

    public ExchangeRateProviderRepositoryInMemory()
    {
        _currencyRates = new Dictionary<DateTime, ISet<ExchangeRate>>();
    }

    public void UpsertExchangeRate(DateTime exchangeDate, ISet<ExchangeRate> exchangeRates)
    {
        var key = exchangeDate.Date;
        if (_currencyRates.ContainsKey(key))
        {
            _currencyRates[key] = exchangeRates;
            return;   
        }

        _currencyRates.Add(key, exchangeRates); 
    }

    public Task<IEnumerable<ExchangeRate>> GetAllFxRates(DateTime exchangeRateDate)
    {
        var key = exchangeRateDate.Date;
        if (_currencyRates.ContainsKey(key))
        {
            return Task.FromResult(_currencyRates[key].AsEnumerable<ExchangeRate>());
        }

        var latestKey = _currencyRates.Keys.Where(date => date <= key).OrderByDescending(date => date).FirstOrDefault();

        return Task.FromResult<IEnumerable<ExchangeRate>>(latestKey == new DateTime() ? new List<ExchangeRate>() : _currencyRates[latestKey]);
    }

    public Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime From, DateTime To)
    {
        var result = new List<ExchangeRate>();
        for (var date = From.Date; date <= To.Date; date = date.AddDays(1))
        {
            var key = date;

            if (_currencyRates.ContainsKey(key))
            {
                result.Add(_currencyRates[key].First(rate => rate.SourceCurrency == sourceCurrency && rate.TargetCurrency == targetCurrency));
            }
        }

        return Task.FromResult<IEnumerable<ExchangeRate>>(result);
    }
}
