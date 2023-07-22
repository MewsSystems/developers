using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Configurations;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ConsoleApplication : BackgroundService
{
    private readonly IRequestClient<GetExchangeRatesForCurrenciesQuery> _getExchangeRatesRequestClient;
    private readonly IOptions<AppConfigurations> _appConfigurations;

    public ConsoleApplication(IOptions<AppConfigurations> appConfigurations, IRequestClient<GetExchangeRatesForCurrenciesQuery> getExchangeRatesRequestClient)
    {
        _appConfigurations = appConfigurations;
        _getExchangeRatesRequestClient = getExchangeRatesRequestClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            try
            {
                var currencies = NonEmptyList<Currency>.CreateUnsafe(_appConfigurations.Value.ValidCurrencies);
                var getExchangeRatesResponse = await _getExchangeRatesRequestClient
                    .GetResponse<NonNullResponse<IEnumerable<ExchangeRate>>>(new GetExchangeRatesForCurrenciesQuery(currencies), stoppingToken);
                WriteResponseToConsole(getExchangeRatesResponse.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{exception.Message}'.");
                if (exception is ArgumentException)
                {
                    Console.WriteLine("Please check that your configuration contains ISO-4217 compliant currency codes.");
                }
            }
        }while(Console.ReadKey().Key != ConsoleKey.Escape);

        await Task.Delay(1000, stoppingToken);
    }

    private void WriteResponseToConsole(NonNullResponse<IEnumerable<ExchangeRate>> response)
    {
        Console.Clear();
        if (response.IsSuccess)
        {
            Console.WriteLine($"Successfully retrieved {response.Content.Count()} exchange rates:\n");
            foreach (var rate in response.Content)
            {
                Console.WriteLine(rate.ToString());
            }

            if (_appConfigurations.Value.InvalidCurrenciesCount > 0)
            {
                var wordTerminator = _appConfigurations.Value.InvalidCurrenciesCount > 1 ? "ies" : "y";
                response.Warnings.Add($"Found {_appConfigurations.Value.InvalidCurrenciesCount} invalid currenc{wordTerminator} in configuration.");
            }
        }
        else
        {
            Console.WriteLine("Could not retrieve exchange rates.");
        }

        if (response.Errors.Any())
        {
            Console.WriteLine("\nErrors:");
            foreach (var error in response.Errors)
            {
                Console.WriteLine(error);
            }
        }        

        if (response.Warnings.Any())
        {
            Console.WriteLine("\nWarnings:");
            foreach (var warning in response.Warnings)
            {
                Console.WriteLine(warning);
            }
        }
        Console.WriteLine("\nPress any key to refresh or ESC to exit.");
    }
}