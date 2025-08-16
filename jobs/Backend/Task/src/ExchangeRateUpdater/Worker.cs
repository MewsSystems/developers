namespace ExchangeRateUpdater;

public class Worker : BackgroundService
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;

    public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger,
        IExchangeRateProvider exchangeRateProvider)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _exchangeRateProvider = exchangeRateProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
            return;

        await DoWork(stoppingToken);

        _hostApplicationLifetime.StopApplication();
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        IEnumerable<Currency> currencies = new[]
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

        var rates = (await _exchangeRateProvider.GetExchangeRatesAsync(currencies, stoppingToken)).ToList();

        _logger.LogInformation("Successfully retrieved {RatesCount} exchange rates: {Rates}", rates.Count, rates);
    }
}