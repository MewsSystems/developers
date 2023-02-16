using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.ExchangeRateSources.CNB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            using IHost host = CreateHostBuilder(args).Build();

            host.RunAsync();
            PrintCurrencies(host.Services).Wait();

            host.StopAsync().Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }

    private static async Task PrintCurrencies(IServiceProvider serviceProvider)
    {
        try
        {
            var currencyOptions = serviceProvider.GetService<IOptions<CurrencyOptions>>().Value;
            var currencies = currencyOptions.Currencies.Select(c => new Currency(c));
            var exchangeRatePrinter = serviceProvider.GetService<IExchangeRatePrinter>();
            await exchangeRatePrinter.PrintRates(currencies);
        }
        catch (Exception e)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogError("Failed to print exchange rates", e);
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .Configure<CurrencyOptions>(context.Configuration.GetSection("CurrencyOptions") ?? throw new Exception("Missing currency options"))
                .Configure<CNBSourceOptions>(context.Configuration.GetSection("CNBSourceOptions") ?? throw new Exception("Missing CNB source options"))
                .AddSingleton<IExchangeRateSource, CNBExchangeRateSource>()
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddTransient<IExchangeRatePrinter, ExchangeRatePrinter>();

}
