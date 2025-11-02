using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Services.RateExporters;
using ExchangeRateUpdater.Services.RateProviders;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExchangeRateUpdater;

/// <summary>
///     Entry point of the application that configures logging, dependency injection,
///     and orchestrates the exchange rate update process.
/// </summary>
public static class Program
{
    public static async Task Main(string[] args)
    {
        var config = ConfigurationLoader.Load();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(config.GetLogLevel())
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen)
            .CreateLogger();

        try
        {
            config.Validate();

            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => { builder.AddSerilog(Log.Logger); })
                .AddDbContext<ExchangeRateDbContext>()
                .AddSingleton<IRepository<ExchangeRateEntity>, Repository<ExchangeRateEntity>>()
                .AddSingleton(config)
                .AddSingleton<Application.Application>();

            switch (config.ProviderType)
            {
                case RateProviderType.Csv:
                    serviceCollection.AddSingleton<IExchangeRateProvider, CzechNationalBankCsvExchangeRateProvider>();
                    break;
                case RateProviderType.Rest:
                    serviceCollection
                        .AddSingleton<IExchangeRateProvider, CzechNationalBankRestApiExchangeRateProvider>();
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported provider type: {config.ProviderType}");
            }

            switch (config.ExporterType)
            {
                case RateExporterType.Console:
                    serviceCollection.AddSingleton<IExchangeRateExporter, ConsoleExchangeRateExporter>();
                    break;
                case RateExporterType.Database:
                    serviceCollection.AddSingleton<IExchangeRateExporter, DatabaseExchangeRateExporter>();
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported exporter type: {config.ExporterType}");
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<Application.Application>();
            await app.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Application terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}