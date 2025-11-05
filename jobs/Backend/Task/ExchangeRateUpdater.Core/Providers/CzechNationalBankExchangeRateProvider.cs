using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

public class CzechNationalBankExchangeRateProvider(
        IExchangeRateVendor exchangeRateVendor
    ) : IExchangeRateProvider
{
    private const string BaseCurrencyCode = "CZK";

    public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        currencies = currencies.ToArray();
        if (!currencies.Any()) return [];

        var rates = await exchangeRateVendor.GetExchangeRates(BaseCurrencyCode);

        return rates
                .Where(rate => currencies.Any(currency => currency.ToString() == rate.TargetCurrency.ToString()))
                .ToList();
    }
}