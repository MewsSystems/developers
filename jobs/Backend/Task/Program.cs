using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Providers.CNB;
using ExchangeRateUpdater.Providers.CNB.Parser.Xml;
using ExchangeRateUpdater.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly IEnumerable<Currency> Currencies = new[]
    {
        new Currency("USD"),
        new Currency("NZD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public static async Task Main(string[] args)
    {
        try
        {
            var exchangeRateProvider = SetUpAndGetExchangeProvider();

            var rates = (await exchangeRateProvider.GetExchangeRatesAsync(Currencies)).ToList();

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }

    private static IExchangeRateProvider SetUpAndGetExchangeProvider()
    {
        var host = CreateDefaultBuilder().Build();

        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var rateProvider = provider.GetRequiredService<IExchangeRateProvider>();
        return rateProvider;
    }

    private static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services
                    .AddLogging()
                    .AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>()
                    .AddTransient<ICNBExchangeRateService, CNBIcnbExchangeRateService>()
                    .AddTransient<IXmlExchangeRateParser, CNBXmlXmlExchangeRateParser>()
                    .AddHttpClient(nameof(CNBIcnbExchangeRateService)).AddPolicyHandler(PollyRetryHelper.GetRetryPolicy());
            });
    }
}