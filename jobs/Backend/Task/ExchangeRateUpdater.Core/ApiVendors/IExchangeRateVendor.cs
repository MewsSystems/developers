using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.ApiVendors;

public interface IExchangeRateVendor
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates(string baseCurrencyCode, IEnumerable<Currency> currencies);
}