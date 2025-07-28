using System.Diagnostics;
using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Polly;
using Serilog.Context;

namespace Mews.ExchangeRateUpdater.Fetcher;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider provider, ILogger<Worker> logger)
    {
        _provider = provider;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var traceId = Guid.NewGuid().ToString("N");

            using (LogContext.PushProperty("TraceId", traceId))
            using (var activity = new Activity("FetchRates").Start())
            {
                await ExecuteFetchWithRetryAsync(stoppingToken);
                await WaitUntilNextRunAsync(stoppingToken);
            }
        }
    }

    private async Task ExecuteFetchWithRetryAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var fetchUseCase = scope.ServiceProvider.GetRequiredService<IFetchExchangeRatesUseCase>();
        
        _logger.LogInformation("Starting scheduled fetch at: {Time}", DateTimeOffset.Now);

        var retryPolicy = Policy
            .Handle<Exception>(ex => ex is not RatesAlreadyExistException)
            .WaitAndRetryForeverAsync(
                sleepDurationProvider: _ => TimeSpan.FromMinutes(5),
                onRetry: (ex, delay, _) =>
                {
                    _logger.LogWarning(ex, "Retrying in {Delay} due to error", delay);
                });

        try
        {
            await retryPolicy.ExecuteAsync(() => TryFetchAsync(fetchUseCase, stoppingToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "All retries failed. Skipping this run.");
        }
    }

    private async Task TryFetchAsync(IFetchExchangeRatesUseCase useCase, CancellationToken ct)
    {
        try
        {
            await useCase.ExecuteAsync(ct, forceUpdate: false);
            _logger.LogInformation("Fetch completed successfully.");
        }
        catch (RatesAlreadyExistException)
        {
            // Already logged in Application layer
        }
    }

    private async Task WaitUntilNextRunAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var nextRun = now.Date.AddDays(1).AddHours(13).AddMinutes(30); // 13:30 UTC next day
        var delay = nextRun - now;

        _logger.LogInformation("Next fetch scheduled at {NextRun} (in {Delay})", nextRun, delay);
        await Task.Delay(delay, ct);
    }
}