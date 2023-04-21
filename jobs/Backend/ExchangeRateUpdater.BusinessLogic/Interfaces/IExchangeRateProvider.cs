using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdater.BusinessLogic.Interfaces
{
    public interface IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, Currency targetCurrency);
    }
}
