namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly ExchangeRateService _exchangeRateService;
    private IEnumerable<ExchangeRate> _cachedExchangeRates;
    private DateTime _cacheExpiration;

    public ExchangeRateProvider(ExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
        _cacheExpiration = DateTime.MinValue;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        if (_cachedExchangeRates != null && DateTime.UtcNow < _cacheExpiration)
        {
            return _cachedExchangeRates.Where(rate => currencies.Contains(rate.SourceCurrency) && currencies.Contains(rate.TargetCurrency));
        }

        var exchangeRatesSource = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        var result = await _exchangeRateService.GetExchangeRatesData(exchangeRatesSource);
        _cachedExchangeRates = ExchangeRateParser.Parse(result);
        _cacheExpiration = DateTime.UtcNow.AddHours(1);

        return _cachedExchangeRates.Where(rate => currencies.Contains(rate.SourceCurrency) && currencies.Contains(rate.TargetCurrency));
    }
}
