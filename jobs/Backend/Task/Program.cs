using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater;

public static class Program
{
    private static IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<IExchangeRateService, ExchangeRateService>(client =>
                {
                    client.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");  //TODO: WILL NEED TO MOVE TO APPSETTINGS ONCE CREATED
                });

                services.AddScoped<IExchangeRateService, ExchangeRateService>();
                services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

                OpenTelemetryConfiguration.ConfigureOpenTelemetry(services);
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddOpenTelemetry(options =>
                    {
                        options.IncludeScopes = true;
                        options.ParseStateValues = true;
                        options.IncludeFormattedMessage = true;
                    });
                });
            })
            .Build();


        
        //try
        //{
        //    var provider = new ExchangeRateProvider();
        //    var rates = provider.GetExchangeRates(currencies);

        //    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        //    foreach (var rate in rates)
        //    {
        //        Console.WriteLine(rate.ToString());
        //    }
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        //}

        //Console.ReadLine();
    }
}
