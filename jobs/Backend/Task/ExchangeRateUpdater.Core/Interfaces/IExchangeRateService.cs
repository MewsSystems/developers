using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Models;

public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, Maybe<DateTime> date);
}
