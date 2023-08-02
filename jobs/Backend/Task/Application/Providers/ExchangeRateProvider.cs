using Application.Abstractions;
using Domain.Model;
using ExchangeRateUpdater;

namespace Application.Providers;

public class ExchangeRateProvider
{
    private readonly IExchangeRatesService _exchangeRatesService;

    public ExchangeRateProvider(IExchangeRatesService exchangeRatesService)
    {
        _exchangeRatesService = exchangeRatesService;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRateDetails>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var latestRates = await _exchangeRatesService.GetCashedExchangeRatesAsync();
        return latestRates.Where(er => currencies.Select(c => c.Code).Contains(er.CurrencyCode));
    }

    /// <summary>
    /// Returns all available exchange rates downloaded from CNB API.
    /// </summary>
    public async Task<IEnumerable<ExchangeRateDetails>> GetExchangeRatesAsync()
    {
        return await _exchangeRatesService.GetCashedExchangeRatesAsync();
    }
}