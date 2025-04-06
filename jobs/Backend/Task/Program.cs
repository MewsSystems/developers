using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;
public static class Program
{
    private static IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
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
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        
        var services = new ServiceCollection();
        
        ConfigureServices(services, configuration);

        var serviceProvider = services.BuildServiceProvider();
        
        try
        {
            var provider = (IExchangeRateProvider)serviceProvider.GetService(typeof(IExchangeRateProvider));
            var rates = await provider.GetExchangeRates(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }
    
    private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExchangeRateProviderOptions>(configuration.GetSection(ExchangeRateProviderOptions.Key));
        
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
    }
}
