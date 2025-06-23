using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastucture.Extensions;

namespace ExchangeRateUpdater.Infrastucture;

public class ExchangeRateProvider(ApiClient apiClient) : IExchangeRateProvider
{
    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>
    public IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var set = currencies.ToHashSet();

        return apiClient.GetAllExchangeRates().WhereAsync(r => set.Contains(r.SourceCurrency));
    }

    public IAsyncEnumerable<ExchangeRate> GetAllExchangeRates() => apiClient.GetAllExchangeRates();
}