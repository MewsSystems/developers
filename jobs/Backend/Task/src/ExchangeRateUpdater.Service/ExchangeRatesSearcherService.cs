using Domain.Entities;
using Domain.Ports;

namespace ExchangeRatesSearcherService;

public class ExchangeRatesSearcherService : IExchangeRatesSearcher
{
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        throw new NotImplementedException();
    }
}