using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public sealed class ExchangeRateService : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IExchangeRatePrinter _exchangeRatePrinter;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly IOptions<CurrencyOptions> _currencyOptions;

    public ExchangeRateService(IHostApplicationLifetime hostApplicationLifetime, IExchangeRatePrinter exchangeRatePrinter, ILogger<ExchangeRateService> logger, IOptions<CurrencyOptions> currencyOptions)
    {
        _exchangeRatePrinter = exchangeRatePrinter;
        _logger = logger;
        _currencyOptions = currencyOptions;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currencies = _currencyOptions.Value.Currencies.Select(c => new Currency(c));
        try
        {
            await _exchangeRatePrinter.PrintRates(currencies);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to print exchange rates", e);
            throw;
        }
        _hostApplicationLifetime.StopApplication();
    }
}
