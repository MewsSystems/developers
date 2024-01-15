using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;
using ExchangeRateUpdater.ExchangeRateProvider;

namespace ExchangeRateUpdater.CnbExchangeRateProvider;

public class CnbExchangeRateProvider : IExchangeRateProvider
{
    private readonly IApiClient _apiClient;
    private static readonly Currency CzkCurrency = new("CZK");

    public CnbExchangeRateProvider(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var currencyCodes = currencies.Select(c => c.Code).ToHashSet();
        if (currencyCodes.Count == 0)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var rates = await _apiClient.GetDailyExchangeRatesAsync();

        return rates?.Rates?
                   .Where(r => currencyCodes.Contains(r.CurrencyCode))
                   .Select(ToExchangeRate)
                   .OrderBy(r => r.SourceCurrency.Code)
                   .ToArray()
               ?? Enumerable.Empty<ExchangeRate>();
    }

    private static ExchangeRateProvider.ExchangeRate ToExchangeRate(ExchangeRateApiModel api) =>
        new(new Currency(api.CurrencyCode), CzkCurrency, api.Rate / api.Amount);
}