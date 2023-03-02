using CommunityToolkit.Diagnostics;
using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
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
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly IOptions<CurrencyOptions> _currencyOptions;

    public ExchangeRateService(IHostApplicationLifetime hostApplicationLifetime,
        IExchangeRatePrinter exchangeRatePrinter,
        IExchangeRateProvider exchangeRateProvider,
        ILogger<ExchangeRateService> logger,
        IOptions<CurrencyOptions> currencyOptions)
    {
        Guard.IsNotNull(currencyOptions.Value.Currencies);

        _exchangeRatePrinter = exchangeRatePrinter;
        _logger = logger;
        _currencyOptions = currencyOptions;
        _hostApplicationLifetime = hostApplicationLifetime;
        _exchangeRateProvider = exchangeRateProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var currencies = _currencyOptions.Value.Currencies.Select(c => new Currency(c));
            var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);
            _exchangeRatePrinter.PrintRates(rates);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to print exchange rates", e);
            throw;
        }
        _hostApplicationLifetime.StopApplication();
    }
}
