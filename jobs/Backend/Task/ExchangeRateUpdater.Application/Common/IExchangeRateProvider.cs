using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Application.Common;

public interface IExchangeRateProvider
{
    Task<IList<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null);
}