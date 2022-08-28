using ExchangeRateUpdater.Models.Rates;

namespace ExchangeRateUpdater;

public class CNBRateProviderService : IExchangeRateProvider
{
    readonly ILogger<CNBRateProviderService> _logger;
    readonly AppConfig _appConfig;
    readonly HttpClient _httpClient;

    public CNBRateProviderService(ILogger<CNBRateProviderService> logger, IOptions<AppConfig> appConfig, HttpClient httpClient)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
        _httpClient = httpClient;
        if (string.IsNullOrWhiteSpace(_appConfig.Uri)) throw new NullReferenceException($"expected valid '{nameof(_appConfig.Uri)}' for rate retrieval");
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        _logger.LogDebug("requesting new data from exchange rate source uri '{uri}'", _appConfig.Uri);
        var xml = await _httpClient.GetStringAsync(_appConfig.Uri);
        _logger.LogDebug("deserialising exchange rate XML '{xml}'", xml);
        var obj = xml.DeserializeToObject<kurzy>();
        var l = new List<ExchangeRate>();
        foreach (var line in obj.tabulka.radek)
        {
            var targetCurrency = new Currency(line.kod);
            if (_appConfig.CurrencyCodesFilter is object
                && _appConfig.CurrencyCodesFilter.FirstOrDefault(p => p.Equals(targetCurrency.Code, StringComparison.OrdinalIgnoreCase)) is object)//TODO: dictionary lookup is faster
            {
                var toRate = new ExchangeRate(new Currency(_appConfig.BaseCurrencyCode), targetCurrency, line.kurzUseable);
                l.Add(toRate);
            }
        }
        return l;
    }
}