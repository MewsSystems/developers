using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

//load configuration from appSettings.json
var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: false)
    .Build();

//initiate the host and add services, options & httpclient
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) => services
        .AddSingleton<IExchangeRateProvider, CNBRateProviderService>() //Note: can be swapped with BOERateProviderService if required...
        .Configure<AppConfig>(configuration.GetSection(AppConfig.SectionKey))
        .AddHttpClient())
    .Build();

await RetrieveRates(host.Services);

await host.RunAsync();

static async Task RetrieveRates(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var exchangeRateProvider = provider.GetRequiredService<IExchangeRateProvider>();
    var logger = provider.GetRequiredService<ILogger<IExchangeRateProvider>>();

    try
    {
        var rates = await exchangeRateProvider.GetExchangeRates();
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }
    }
    catch (Exception e)
    {
        var msg = $"Could not retrieve exchange rates: '{e.Message}'.";
        logger.LogError(msg);
        //Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
    }

    Console.WriteLine("Hit any key to exit...");
    Console.ReadLine();
}