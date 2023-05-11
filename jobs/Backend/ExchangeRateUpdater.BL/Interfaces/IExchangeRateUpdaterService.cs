using ExchangeRateUpdater.BL.Models;

namespace ExchangeRateUpdater.BL.Interfaces
{
    public interface IExchangeRateUpdaterService
    {
        IEnumerable<ExchangeRate> GetExchangeRateMappedFromSource(IEnumerable<Currency> currencies, string URL);
    }
}
