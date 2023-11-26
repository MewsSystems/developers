using Domain.Entities;
using Domain.Ports;

namespace Adapter.InMemory.Repositories;

public class ExchangeRatesRepositoryInMemory : IExchangeRatesRepository
{
    private Dictionary<DateTime, List<ExchangeRate>> _exchangeRates;
    
    public ExchangeRatesRepositoryInMemory()
    {
        _exchangeRates = new Dictionary<DateTime, List<ExchangeRate>>();
    }

    public async Task<List<ExchangeRate>> GetDailyExchangeRatesAsync(DateTime dateToRequest, CancellationToken cancellationToken)
    {
        if (_exchangeRates.ContainsKey(dateToRequest))
        {
            return _exchangeRates[dateToRequest];
        }

        return null;
    }

    public void AddExchangeRate(ExchangeRate exchangeRate, DateTime date)
    {
        if (!_exchangeRates.ContainsKey(date))
        {
            _exchangeRates.Add(date, new List<ExchangeRate>());
        }
        
        _exchangeRates[date].Add(exchangeRate);
    }
}