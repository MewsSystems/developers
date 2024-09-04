namespace ExchangeRateUpdater;

public class ExchangeRateWorker : BackgroundService
{
    private readonly ExchangeRateProvider _exchangeRateProvider;
    private readonly IHostApplicationLifetime _appLifetime;

    public ExchangeRateWorker(ExchangeRateProvider exchangeRateProvider, IHostApplicationLifetime appLifetime)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _appLifetime = appLifetime;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var rates = (await _exchangeRateProvider.GetExchangeRates()).ToArray();

            Console.WriteLine($"Successfully retrieved {rates.Length} exchange rates:");

            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
        finally
        {
            _appLifetime.StopApplication();
        }
    }
}