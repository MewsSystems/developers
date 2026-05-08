using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Services.Interfaces;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    
}