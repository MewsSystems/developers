using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Providers;

public class ExchangeRateProvider
{
    private readonly IExchangeRateProviderClient _client;

    public ExchangeRateProvider(IExchangeRateProviderClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var response = await _client.GetExchangeRatesAsync();
        return response.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));
    }
}