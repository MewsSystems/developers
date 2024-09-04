namespace ExchangeRateUpdater;

public class ExchangeRateWorker : BackgroundService
{
    private readonly ExchangeRateProvider _exchangeRateProvider;
    private readonly ILogger<ExchangeRateWorker> _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public ExchangeRateWorker(ExchangeRateProvider exchangeRateProvider, ILogger<ExchangeRateWorker> logger, IHostApplicationLifetime appLifetime)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _logger = logger;
        _appLifetime = appLifetime;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var rates = (await _exchangeRateProvider.GetExchangeRates()).ToArray();

            _logger.LogInformation("Successfully retrieved {numberOfRates} exchange rates:", rates.Length);

            foreach (var rate in rates)
            {
                _logger.LogInformation("{rate}", rate.ToString());
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve exchange rates.");
        }
        finally
        {
            _appLifetime.StopApplication();
        }
    }
}