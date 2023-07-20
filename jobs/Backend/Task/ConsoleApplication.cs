using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Components.Queries;
using ExchangeRateUpdater.Application.Components.Responses;
using ExchangeRateUpdater.Application.Configurations;
using ExchangeRateUpdater.Domain.Types;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class ConsoleApplication : BackgroundService
{
    private readonly IRequestClient<GetExchangeRatesQuery> _getExchangeRatesRequestClient;
    private readonly IEnumerable<Currency> _currencies;

    public ConsoleApplication(IOptions<AppConfigurations> appConfigurations, IRequestClient<GetExchangeRatesQuery> getExchangeRatesRequestClient)
    {
        _getExchangeRatesRequestClient = getExchangeRatesRequestClient;
        _currencies = appConfigurations.Value.Currencies;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var getExchangeRatesResponse = await _getExchangeRatesRequestClient.GetResponse<GetExchangeRatesResponse>(new GetExchangeRatesQuery(_currencies), stoppingToken);
            var rates = getExchangeRatesResponse.Message.ExchangeRates;

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        await Task.Delay(1000, stoppingToken);
    }
}