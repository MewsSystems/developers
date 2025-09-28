using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;

public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);
}
