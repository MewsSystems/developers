using System;
using ExchangeRateUpdater.application;
using ExchangeRateUpdater.config;
using ExchangeRateUpdater.services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ExchangeRateUpdater;

public static class Program
{
    public static void Main(string[] args)
    {
        var config = ConfigurationLoader.Load();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(config.GetLogLevel())
            .WriteTo.Console()
            .CreateLogger();

        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => { builder.AddSerilog(Log.Logger); })
            .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
            .AddSingleton(config)
            .AddSingleton<Application>()
            .BuildServiceProvider();

        try
        {
            config.Validate();

            var app = serviceProvider.GetRequiredService<Application>();
            app.Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Application terminated unexpectedly");
        }
        finally
        {
            serviceProvider.Dispose();
            Log.CloseAndFlush();
        }
    }
}