using ExchangeRatesService.Models;
using ExchangeRatesService.Providers;
using ExchangeRatesService.Providers.Interfaces;

namespace ExchangeRatesViewer;

public class ExchangeRatesPrinter(ExchangeRateProvider exProvider, ForexRateProvider fxProvider, IRatesConverter converterService, 
    ILogger<ExchangeRatesPrinter> logger): IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        const decimal amount = 10;
     
        logger.LogInformation("++++++++++ Retrieving exchange Rates +++++++++++");
        Console.WriteLine();
        await foreach (var rate in exProvider.GetRatesAsync(CurrencyIterator.Currencies))
        {
            logger.LogInformation(rate.ToString());
        }
        
        logger.LogInformation($"++++++++++ Calculated exchange rates conversion with base currency CZK and source amount {amount}+++++++++++");
        await foreach (var rate in exProvider.GetRatesReverseAsync(CurrencyIterator.Currencies, amount))
        {
            logger.LogInformation(rate.ToString());
        }
                
        logger.LogInformation("++++++++++ Calculated exchange Rates with base currency CKZ +++++++++++");
        await foreach (var rate in exProvider.GetRatesAsync(CurrencyIterator.Currencies))
        {
            var converter = converterService.GetConversion(rate.SourceCurrency, rate.TargetCurrency, rate.Value,amount);
            logger.LogInformation($"Converted amount: {amount} with exchange rate {rate} is {converter.Result}");
        }
                
        Console.WriteLine("++++++++++ Forex Rate Provider ++++++++++++");
        await foreach (var rate in fxProvider.GetRatesAsync(CurrencyIterator.Currencies))
        {
            var converter = converterService.GetConversion(rate.SourceCurrency, rate.TargetCurrency, rate.Value,amount);
            logger.LogInformation($"Converted amount: {amount} with exchange rate {rate} is {converter.Result}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}