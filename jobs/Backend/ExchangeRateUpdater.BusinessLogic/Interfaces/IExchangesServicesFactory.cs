using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdater.BusinessLogic.Interfaces
{
    public interface IExchangesServicesFactory
    {
        IExchangeService GetExchangeService(Currency targetCurrency);
    }
}
