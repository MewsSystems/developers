namespace ExchangeRateUpdater.Console;

public class ExchangeRateProvider(ICNBApiClient apiClient, ICache cache)
{
    private readonly ICNBApiClient _apiClient = apiClient;
    private readonly ICache _cache = cache;

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var result = new List<ExchangeRate>();

        const string cacheKey = "ExratesDailyResponse";
        var exratesApiResponse = _cache.GetData<ExratesDailyResponse>(cacheKey);

        if (exratesApiResponse == null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            exratesApiResponse = await _apiClient.GetDailyExrates(today, CancellationToken.None);
            _cache.SetData(cacheKey, exratesApiResponse);
        }

        foreach (var currency in currencies)
        {
            var exrate = exratesApiResponse.Rates.FirstOrDefault(x =>
                x.CurrencyCode == currency.Code
            );

            if (exrate == null)
            {
                continue;
            }

            result.Add(MapExrate(exrate));
        }

        return result;
    }

    private static ExchangeRate MapExrate(ExrateApiModel exrateApi) =>
        new(new Currency("CZK"), new Currency(exrateApi.CurrencyCode), (decimal)exrateApi.Rate);
}
