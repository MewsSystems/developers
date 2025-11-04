using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;

public class CurrencyApiExchangeRateVendor : IExchangeRateVendor
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        throw new NotImplementedException();
    }
}