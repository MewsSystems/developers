using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateRepository
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
}