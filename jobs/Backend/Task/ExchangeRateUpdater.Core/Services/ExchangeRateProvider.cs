using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infra;
using ExchangeRateUpdater.Infra.Http;
using ExchangeRateUpdater.Infra.Models;
using Microsoft.Extensions.Logging;
using ExchangeRate = ExchangeRateUpdater.Core.Models.ExchangeRate;

namespace ExchangeRateUpdater.Core.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private static readonly Currency DefaultTargetCurrency = new Currency("CZH");
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly ICnbHttpClient _cbnHttpClient;
    
    public ExchangeRateProvider(ICnbHttpClient cbnHttpClient, ILogger<ExchangeRateProvider> logger)
    {
        _cbnHttpClient = cbnHttpClient;
        _logger = logger;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var result = await _cbnHttpClient.GetExchangeRatesAsync();
        return result.Match(
            success => HandleSuccess(success, currencies),
            HandleError);
    }

    private static IEnumerable<ExchangeRate> HandleSuccess(ExchangeRateResponse response, IEnumerable<Currency> currencies)
    {
        var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));
        
        return response.Rates
            .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
            .Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode),
                DefaultTargetCurrency,
                rate.Rate / rate.Amount));
    }

    private IEnumerable<ExchangeRate> HandleError(HttpError error)
    {
        _logger.LogError($"Error fetching exchange rates from cnb-api. Http Status Code: {error.HttpStatusCode}. Error Response: {error.Error}");
        return Enumerable.Empty<ExchangeRate>();
    }
}