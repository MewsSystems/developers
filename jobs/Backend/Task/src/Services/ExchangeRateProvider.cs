using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly ICnbExchangeRateService _cnbService;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private static readonly Currency BaseCurrency = new(CnbConstants.BaseCurrencyCode);

    public ExchangeRateProvider(ICnbExchangeRateService cnbService, ILogger<ExchangeRateProvider> logger)
    {
        _cnbService = cnbService ?? throw new ArgumentNullException(nameof(cnbService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        if (currencies == null)
        {
            throw new ArgumentNullException(nameof(currencies));
        }

        var currencyList = currencies.ToList();
        if (currencyList.Count == 0)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var cnbRates = await _cnbService.GetDailyRatesAsync(cancellationToken: cancellationToken);
        if (cnbRates == null)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var result = new List<ExchangeRate>();

        foreach (var currency in currencyList)
        {
            if (currency == null)
            {
                continue;
            }

            if (currency.Code.Equals(BaseCurrency.Code, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (cnbRates.TryGetValue(currency.Code, out var rateValue) && rateValue > 0)
            {
                result.Add(new ExchangeRate(BaseCurrency, currency, rateValue));
            }
        }

        _logger.LogInformation("Retrieved {Count} exchange rates", result.Count);
        return result;
    }
}
