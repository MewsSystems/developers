using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var exchangeRates = await _exchangeRateRepository.GetExchangeRates();
        return exchangeRates.Where(rate => currencies.Any(c => c.Code == rate.SourceCurrency.Code));
    }
}