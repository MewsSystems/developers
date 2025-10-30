using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.config;
using ExchangeRateUpdater.services;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.application;

public class Application
{
    private readonly AppConfiguration _appConfiguration;
    private readonly ILogger<Application> _logger;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IExchangeRateExporter _exchangeRateExporter;

    public Application(ILogger<Application> logger, AppConfiguration appConfiguration, IExchangeRateProvider exchangeRateProvider, IExchangeRateExporter exchangeRateExporter)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
        _exchangeRateProvider = exchangeRateProvider;
        _exchangeRateExporter = exchangeRateExporter;
    }

    public async Task RunAsync()
    {
        try
        {
            var currencies = _appConfiguration.GetCurrencies();
            _logger.LogInformation("Starting exchange rate retrieval for currencies: {Currencies}", currencies);

            var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);
            await _exchangeRateExporter.ExportExchangeRatesAsync(rates);
            
            _logger.LogInformation("Exchange rate retrieval and export completed successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve exchange rates");
        }
    }
}