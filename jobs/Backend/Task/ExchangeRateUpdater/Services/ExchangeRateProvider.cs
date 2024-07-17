using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateProvider : IHostedService
{
    private readonly ICnbApiClient _cnbApiClient;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly IHost _host;

    private static readonly IEnumerable<Currency> currencies = new[]
    {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
    };

    public ExchangeRateProvider(
        ILogger<ExchangeRateProvider> logger,
        ICnbApiClient cnbApiClient,
        IHost host)
    {
        _logger = logger;
        _cnbApiClient = cnbApiClient;
        _host = host;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var rates = await GetExchangeRates(cancellationToken);

        _logger.LogInformation("Successfully retrieved {count} exchange rates", rates.Count());
        foreach (var rate in rates)
        {
            _logger.LogInformation("Rate: {rate}", rate.ToString());
        }

        await _host.StopAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken token)
    {
        var rates = await _cnbApiClient.GetExchangeRatesFromCnbApi(token);
        List<ExchangeRate> exchangeRates = new();
        foreach (var currency in currencies)
        {
            // This dictionary provides a O(1) retrieval from the list of currencies. 
            if (rates.TryGetValue(currency.Code, out DailyExRateItem? item))
            {
                exchangeRates.Add(new(new(currency.Code), new("CZK"), item.Rate));
            }
        }

        // This list only contains currencies that were found in the currency list and the data source.
        return exchangeRates;
    }
}
