using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private const int CacheDurationInHours = 1;
    private DateTime _cacheExpiration;
    private IEnumerable<ExchangeRate> _cachedExchangeRates = Enumerable.Empty<ExchangeRate>();

    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(IExchangeRateService exchangeRateService, ILogger<ExchangeRateProvider> logger)
    {
        _exchangeRateService = exchangeRateService;
        _cacheExpiration = DateTime.MinValue;
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
        if (DateTime.UtcNow > _cacheExpiration)
        {
            try
            {
                var result = await _exchangeRateService.GetExchangeRatesData();

                _cachedExchangeRates = ExchangeRateParser.Parse(result ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving exchange rates: {ex.Message}");

                return Enumerable.Empty<ExchangeRate>();
            }

            _cacheExpiration = DateTime.UtcNow.AddHours(CacheDurationInHours);
        }

        return _cachedExchangeRates
            .Where(rate => currencies.Contains(rate.SourceCurrency) && currencies.Contains(rate.TargetCurrency));
    }
}
