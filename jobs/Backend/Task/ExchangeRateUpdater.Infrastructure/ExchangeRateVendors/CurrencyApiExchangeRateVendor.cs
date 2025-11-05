using System.Net.Http.Json;
using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure.Dtos;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;

public class CurrencyApiExchangeRateVendor(
        HttpClient httpClient
    ) : IExchangeRateVendor
{
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(string baseCurrencyCode, IEnumerable<Currency> currencies)
    {
        string requestUri = $"latest?base_currency={baseCurrencyCode}&currencies={string.Join(",", currencies.Select(currency => currency.ToString()))}";
        var result = await httpClient.GetAsync(requestUri);

        if (!result.IsSuccessStatusCode) return [];
        var exchangeRates = await result.Content.ReadFromJsonAsync<CurrencyApiResponse>();

        return exchangeRates == null ? [] : exchangeRates.Data.Select(rate => new ExchangeRate(new Currency(baseCurrencyCode), new Currency(rate.Key), rate.Value.Value));
    }
}