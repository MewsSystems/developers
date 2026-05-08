using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Interfaces;

public interface IExchangeRateProvider
{
    Task<List<ExchangeRate>> GetExchangeRates(string[] currencies, CancellationToken cancellationToken);
}