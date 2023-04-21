using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdater.BusinessLogic.Interfaces
{
    public interface IExchangeService
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
