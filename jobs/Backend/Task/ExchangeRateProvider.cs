using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateProvider> _logger;
    public ExchangeRateProvider(IExchangeRateService exchangeRateService, ILogger<ExchangeRateProvider> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var exchangeRates = await _exchangeRateService.GetExchangeRateListAsync();

        if (exchangeRates?.Rates == null || !exchangeRates.Rates.Any())
        {
            _logger.LogInformation("No exchange rates available from the service.");
            return Enumerable.Empty<ExchangeRate>();
        }

        var filteredExchangeRates = exchangeRates.Rates
            .Where(rate => currencies.Any(currency => currency.Code == rate.CurrencyCode))
            .Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode),
                new Currency("CZK"),
                rate.Rate / rate.Amount));

        if (!filteredExchangeRates.Any())
        {
            _logger.LogInformation("No matching rates found for chosen currency codes.");
            return Enumerable.Empty<ExchangeRate>();
        }

        return filteredExchangeRates;
    }
}
