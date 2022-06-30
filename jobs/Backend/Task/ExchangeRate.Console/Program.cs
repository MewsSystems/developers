using Common.Exceptions;
using Common.Log.Extensions;
using ExchangeRate.Console.Extensions;
using ExchangeRate.Provider.Cnb.Extensions;
using ExchangeRate.Service.Enums;
using ExchangeRate.Service.Extensions;
using ExchangeRate.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using HttpRequestException = Common.Exceptions.HttpRequestException;

namespace ExchangeRate.Console;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var host = CreateHostBuilder(args);

            var exchangeService = host.Services.GetRequiredService<IExchangeRateService>();

            if (exchangeService is null)
                throw new Exception($" Unable to get {nameof(IExchangeRateService)}!");

            PrintResult(await exchangeService.GetExchangeRates(ProviderSource.Cnb));

            System.Console.ReadLine();
        }
        catch (ConfigurationException ex)
        {
            if (Log.Logger is not null)
            {
                Log.Error(ex, "Configuration is not set correctly");
                return;
            }

            System.Console.WriteLine($"Logger is not initialized - Configuration is not set correctly - {ex.Message}");
        }
        catch (InvalidContentException ex)
        {
            if (Log.Logger is not null)
            {
                Log.Error(ex, ex.Message);
                return;
            }

            System.Console.WriteLine($"Logger is not initialized - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            if (Log.Logger is not null)
            {
                Log.Error(ex, "Error exception when processing request with content");
                return;
            }

            System.Console.WriteLine($"Logger is not initialized - Error exception when processing request with content - {ex.Message}");
        }
        catch (Exception ex)
        {
            if (Log.Logger is not null)
            {
                Log.Error(ex, "Application crashed");
                return;
            }

            System.Console.WriteLine($"Logger is not initialized - Application crashed - {ex.Message}");
        }
    }

    private static IHost CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            // Set all application environment variables
            .ConfigureAppEnvironmentVariables()
            // Set appsettings dependent on environment variable
            .ConfigureAppSettings()
            // Add serilog for all logging
            .AddSerilog()
            .ConfigureServices((context, collection) =>
            {
                collection
                    // Add cache
                    .AddMemoryCache()
                    // Add configurations
                    .AddCnbProviderConfiguration(context.Configuration)
                    // Add services
                    .AddExchangeRateServices();
            })
            .Build()
            // Validates all required configurations
            .ValidateConfigurations();

        return host;
    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (Log.Logger is null || e.ExceptionObject is not Exception ex)
            return;

        Log.Logger.Error(ex, "Application crashed");

        if (e.IsTerminating)
            Log.CloseAndFlush();
    }

    private static void PrintResult(IEnumerable<Models.ExchangeRate> result)
    {
        System.Console.WriteLine($"Successfully retrieved {result.Count()} exchange rates in amount 1:1:");

        foreach (var exchangeRate in result)
            System.Console.WriteLine(exchangeRate.ToString());
    }
}