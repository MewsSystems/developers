using ExchangeRateUpdater;

namespace Application.CzechNationalBank.Providers
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}