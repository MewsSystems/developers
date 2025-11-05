using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.ApiVendors;

public interface IExchangeRateVendor
{
    Task<List<ExchangeRate>> GetExchangeRates(string baseCurrencyCode);
}