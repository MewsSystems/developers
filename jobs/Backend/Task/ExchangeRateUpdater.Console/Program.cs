using ExchangeRateUpdater.ApplicationServices;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

    private static IEnumerable<Currency> Currencies =
    [
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    ];

    public static async Task Main(string[] args)
    {
        // Application configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        // IoC/DI configuration
        var services = new ServiceCollection();
        services.AddApplicationServices();
        services.AddInfrastructureServices(config);

        using var serviceProvider = services.BuildServiceProvider();
        var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

        // Console test
        try
        {
            var rates = (await exchangeRateService.GetTodayExchangeRatesAsync(Currencies)).ToList();

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}