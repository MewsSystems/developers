using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.ExchangeRateSources.CNB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ExchangeRateUpdater;

public sealed class Program
{
    public static void Main(string[] args)
    {
        try
        {
            using IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .Configure<CurrencyOptions>(GetConfig<CurrencyOptions>(context, "CurrencyOptions", "Missing currency options"))
                .Configure<CNBSourceOptions>(GetConfig<CNBSourceOptions>(context, "CNBSourceOptions", "Missing CNB source options"))
                .AddSingleton<IExchangeRateSource, CNBExchangeRateSource>()
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddTransient<IExchangeRatePrinter, ExchangeRatePrinter>()
                .AddHostedService<ExchangeRateService>();

    private static IConfigurationSection GetConfig<T>(HostBuilderContext context, string sectionName, string exceptionMessage)
    {
        var config = context.Configuration.GetSection(sectionName);
        if (config.Get<T>() == null)
        {
            throw new Exception(exceptionMessage);
        }
        return config;
    }
}
