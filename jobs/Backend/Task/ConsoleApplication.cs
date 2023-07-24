using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ConsoleApplication : BackgroundService
{
    private readonly IRequestClient<GetExchangeRatesForCurrenciesQuery> _getExchangeRatesForCurrenciesRequestClient;
    private readonly List<Currency> _configuredCurrencies;

    public ConsoleApplication(IRequestClient<GetExchangeRatesForCurrenciesQuery> getExchangeRatesForCurrenciesRequestClient, IOptions<List<Currency>> configuredCurrencies)
    {
        _getExchangeRatesForCurrenciesRequestClient = getExchangeRatesForCurrenciesRequestClient;
        _configuredCurrencies = configuredCurrencies.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            Console.Clear();

            try
            {
                var (validCurrencies, warningMessage) = _configuredCurrencies.Validate();
                var query = new GetExchangeRatesForCurrenciesQuery(NonEmptyList<ValidCurrency>.CreateUnsafe(validCurrencies));
                var getExchangeRatesResponse = 
                    await _getExchangeRatesForCurrenciesRequestClient.GetResponse<NonNullResponse<IEnumerable<ExchangeRate>>>(query,stoppingToken);
                
                if(warningMessage != null)
                    getExchangeRatesResponse.Message.AddWarningMessage(warningMessage);
                
                getExchangeRatesResponse.Message.PrintToConsole();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{exception.Message}'.");
                if (exception is ArgumentException)
                    Console.WriteLine("Please check that your configuration contains ISO-4217 compliant currency codes.");
            }

            Console.WriteLine("Press any key to refresh or ESC to exit.");

        } while (Console.ReadKey().Key != ConsoleKey.Escape);
        
        Environment.Exit(0);
    }
}