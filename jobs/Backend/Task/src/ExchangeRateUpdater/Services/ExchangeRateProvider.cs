namespace ExchangeRateUpdater.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly string _baseCurrencyCode;
    private readonly IExternalBankApiClient _client;

    public ExchangeRateProvider(IExternalBankApiClient client)
    {
        _client = client;
        // TODO read base currency from settings?
        _baseCurrencyCode = "CZK";
    }

    /// <summary>
    ///     Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    ///     by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    ///     do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    ///     some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        return GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default)
    {
        var rates = await _client.GetDailyExchangeRatesAsync(cancellationToken: cancellationToken);

        var result = new List<ExchangeRate>();

        foreach (var currency in currencies)
        {
            var rate = rates.Rates.FirstOrDefault(r => r.CurrencyCode == currency.Code);

            if (rate is null) continue;

            result.Add(new ExchangeRate(new Currency(currency.Code), new Currency(_baseCurrencyCode), rate.Rate));
        }

        return result;
    }
}