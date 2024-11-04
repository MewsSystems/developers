using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public sealed class HostedProcessingService : IHostedService
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IConsoleManager _consoleManager;
    private readonly ILogger<HostedProcessingService> _logger;

    public HostedProcessingService(
        IExchangeRateProvider exchangeRateProvider,
        IConsoleManager consoleManager,
        ILogger<HostedProcessingService> logger)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _consoleManager = consoleManager;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started retrieving exchange rates");

        try
        {
            var requiredCurrencyCodes = _exchangeRateProvider.GetActualCurrencyCodes();
            var rates = await _exchangeRateProvider.GetExchangeRates(requiredCurrencyCodes, cancellationToken);
            _consoleManager.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");

            foreach (var rate in rates)
            {
                _logger.LogDebug("Successfully retrieved {@rate} exchange rates", rate);
                _consoleManager.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {            
            // Hopefully there are some middleware for exceptions handling which will automatically log this,
            // or we can log something here
            _logger.LogError(e, $"Could not retrieve exchange rates: {e.Message}");
            _consoleManager.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        _logger.LogInformation("Finished retrieving exchange rates");
        _consoleManager.ReadLine();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The exchange rates update was finished");
        return Task.CompletedTask;
    }
}