using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        throw new NotImplementedException();
    }
}