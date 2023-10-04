using ExchangeRatesUpdater;
using ExchangeRatesUpdater.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try {
            IServiceProvider serviceProvider = ConfigureServices();

            var parametersParser = serviceProvider.GetRequiredService<ParametersParser>();
            var exchangeRatesCoordinator = serviceProvider.GetRequiredService<ExchangeRatesCoordinator>();

            var parameters = parametersParser.Parse(args);
            await exchangeRatesCoordinator.GetAndExportExchangeRates(parameters);

            Console.ReadLine();
        } catch (Exception ex) {
            Log.Fatal(ex, "An unhandled exception occurred.");
        } finally {
            Log.CloseAndFlush();
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection serviceCollection = new();

        AddLogging(serviceCollection);
        AddConfiguration(serviceCollection);
        AddServices(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }

    private static void AddLogging(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/ExchangeRateUpdater.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(builder => {
            builder.AddSerilog();
        });
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

        IConfiguration configuration = builder.Build();

        services.Configure<AppConfiguration>(configuration);

        services.AddSingleton(configuration);
    }

    private static void AddServices(IServiceCollection services)
    {
        ExchangeRatesUpdater.Installer.AddServices(services);
        ExchangeRatesFetching.Installer.AddServices(services);
        ExchangeRatesExporting.Installer.AddServices(services);

        services.AddHttpClient();
    }
}
