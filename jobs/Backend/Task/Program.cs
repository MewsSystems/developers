using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Infrastructure.Cnb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            _host = CreateHostBuilder(args).Build();

            _logger = _host.Services.GetService<ILogger<ExchangeRateProvider>>() ??
                      throw new InvalidOperationException($"Could not resolve '{nameof(ILogger)}' and start the app.");

            var exchangeRateProvider = _host.Services.GetService<IExchangeRateProvider>() ??
                                       throw new InvalidOperationException($"Could not resolve '{nameof(IExchangeRateProvider)}' and start the app.");
            var rates = await exchangeRateProvider.GetExchangeRatesAsync();

            Console.WriteLine($"\nSuccessfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'. \nContact application administrator.");
            _logger.LogCritical(e, "Error occured during running the app.");
        }

        Console.ReadLine();
        await _host.StopAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddEventLog();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json");
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<SupportedCurrenciesOptions>(context.Configuration.GetSection(nameof(SupportedCurrenciesOptions)));
                services.Configure<CnbApiOptions>(context.Configuration.GetSection(nameof(CnbApiOptions)));
                services.AddSingleton<IExchangeRatesClient, ExchangeRatesClient>();
                services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddHttpClient<IExchangeRatesClient, ExchangeRatesClient>(conf =>
                {
                    CnbApiOptions options = new();
                    context.Configuration.GetSection(nameof(CnbApiOptions)).Bind(options);
                    conf.BaseAddress = new Uri(options.BaseUrl);
                });
            });

    private static IHost _host = null!;
    private static ILogger<ExchangeRateProvider> _logger = null!;
}