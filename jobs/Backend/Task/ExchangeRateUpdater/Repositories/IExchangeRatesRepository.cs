using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Repositories;

public interface IExchangeRatesRepository
{
    public Task<List<ExchangeRate>> GetExchangeRatesAsync();
}