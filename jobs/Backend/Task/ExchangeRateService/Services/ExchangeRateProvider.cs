using ExchangeRateService.Domain;
using ExchangeRateService.ExternalServices;

namespace ExchangeRateService.Services;

internal class ExchangeRateProvider(ICNBClientService cnbClient) : IExchangeRateProvider
{
    private readonly ICNBClientService _cnbClient = cnbClient ?? throw new ArgumentNullException(nameof(cnbClient));
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, it does not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// it does not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, they are ignored.
    /// </summary>
    public async ValueTask<ExchangeRate[]> GetExchangeRatesAsync(IReadOnlyList<Currency> currencies, CancellationToken cancellationToken = default)
    {
        // TODO add IMemoryCache here as described https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-8.0
        var allExchangeRates = await _cnbClient.GetDailyExchangeRatesAsync(cancellationToken);
        if (!currencies.Any())
            return allExchangeRates;
        
        return allExchangeRates
            .Where(exchangeRate => currencies.Contains(exchangeRate.SourceCurrency)) 
            .ToArray();
    }
}