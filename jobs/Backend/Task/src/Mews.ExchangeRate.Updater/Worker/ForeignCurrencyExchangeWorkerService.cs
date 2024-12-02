using Mews.ExchangeRate.Updater.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Updater.Services.Worker;

/// <summary>
/// This service is intended to periodically update the exchange rates. According to CNB
/// Documentation, the exchange rates are updated every working day at 2:30pm. And the Foreign
/// Exchange Rates are updated every month.
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.IHostedService"/>
public class ForeignCurrencyExchangeWorkerService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;

    public ForeignCurrencyExchangeWorkerService(
        IServiceProvider serviceProvider, ILogger<ForeignCurrencyExchangeWorkerService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Using a background scheduler like Quartz.NET or Hangfire, we can schedule the update of
        // the exchange rates.
        // - Update Currency Exchange Rates every working day at 2:30pm.
        // - Update Foreign Exchange Rates every month.
        _timer = new PeriodicTimer(TimeSpan.FromDays(30));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
        _logger.LogInformation("{service} INITIALIZED", nameof(ForeignCurrencyExchangeWorkerService));
        _ = Task.Run(async () => await ProcessUpdateRatesAsync()); //Fire and forget on init
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("{service} FINISHED", nameof(ForeignCurrencyExchangeWorkerService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await ProcessUpdateRatesAsync();
        }
    }

    private async Task ProcessUpdateRatesAsync()
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var updateService = scope.ServiceProvider.GetRequiredService<IExchangeRateUpdateService>();
            await updateService.RefreshCurrencyExchangeRatesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Updating Foreign Currency Exchange information. {exMessage}", ex.Message);
        }
    }
}