using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.application;
using ExchangeRateUpdater.config;
using ExchangeRateUpdater.services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var config = ConfigurationLoader.Load();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(config.GetLogLevel())
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();


        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => { builder.AddSerilog(Log.Logger); })
            .AddSingleton<IExchangeRateProvider, CzechNationalBankRestApiExchangeRateProvider>()
            .AddSingleton<IExchangeRateExporter, ConsoleExchangeRateExporter>()
            .AddSingleton<IExchangeRateExporter, ConsoleExchangeRateExporter>()
            .AddSingleton(config)
            .AddSingleton<Application>()
            .BuildServiceProvider();

        try
        {
            config.Validate();

            var app = serviceProvider.GetRequiredService<Application>();
            await app.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Application terminated unexpectedly");
        }
        finally
        {
            await serviceProvider.DisposeAsync();
            await Log.CloseAndFlushAsync();
        }
    }
}