using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

 namespace ExchangeRateUpdater.Services;

public interface IExchangeRateProviderService
{
    Task<List<ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies);
}