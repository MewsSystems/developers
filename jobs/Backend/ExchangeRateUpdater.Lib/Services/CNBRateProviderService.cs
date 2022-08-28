using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models.Rates;

namespace ExchangeRateUpdater;

public class CNBRateProviderService : IExchangeRateProvider
{
    readonly ILogger<CNBRateProviderService> _logger;
    readonly AppConfig _appConfig;
    readonly HttpClient _httpClient;
    readonly HashSet<string> _validTargetCurrencies;

    public CNBRateProviderService(ILogger<CNBRateProviderService> logger, IOptions<AppConfig> appConfig, HttpClient httpClient)
    {
        _logger = logger;
        _appConfig = appConfig.Value;
        _httpClient = httpClient;
        if (string.IsNullOrWhiteSpace(_appConfig.Uri))
            throw new NullReferenceException($"expected valid '{nameof(_appConfig.Uri)}' for rate retrieval");
        if (_appConfig.CurrencyCodesFilter is null || !_appConfig.CurrencyCodesFilter.Any())
            throw new NullReferenceException($"expected 1 or more currency codes in '{nameof(_appConfig.CurrencyCodesFilter)}'");
        _validTargetCurrencies = _appConfig.CurrencyCodesFilter.ToHashSet();
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
    {
        //Note: could be IAsyncEnumerable with yield return but no benefit IMO for such a performant function...
        _logger.LogDebug("Requesting new data from exchange rate source uri '{uri}'", _appConfig.Uri);
        var xml = await _httpClient.GetStringAsync(_appConfig.Uri);
        if (string.IsNullOrWhiteSpace(xml))
            throw new RateRetrievalException("XML rate object returned either null or empty");
        _logger.LogDebug("Deserialising exchange rate XML '{xml}'", xml);
        kurzy obj;
        try
        {
            obj = xml.DeserializeToObject<kurzy>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(SerializationHelpers.DeserializeToObject)} failure for xml document '{xml}'");
            throw new RateRetrievalException($"critical failure in xml deserialisation");
        }
        if (!(obj is object && obj.tabulka is object && obj.tabulka.radek is object && obj.tabulka.radek.Length > 0))
            throw new RateRetrievalException("rate object is not populated as expected");
        var l = new List<ExchangeRate>(obj.tabulka.radek.Length);
        foreach (var line in obj.tabulka.radek)
        {
            var targetCurrency = new Currency(line.kod);
            if (_validTargetCurrencies.Contains(targetCurrency.Code, StringComparer.OrdinalIgnoreCase))
                l.Add(new ExchangeRate(new Currency(_appConfig.BaseCurrencyCode), targetCurrency, line.kurzUseable));
        }
        return l;
    }
}