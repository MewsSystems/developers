using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

public class CzechNationalBankExchangeRateProvider(
        IExchangeRateVendor exchangeRateVendor
    ) : IExchangeRateProvider
{
    private const string BaseCurrencyCode = "CZK";

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var currencyConversion = await exchangeRateVendor.GetExchangeRates(BaseCurrencyCode, currencies);

        return currencyConversion;
    }
}