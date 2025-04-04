using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly IEnumerable<Currency> currencies = new[]
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
        // DI setup
        var serviceProvider = ConfigureServices();

        var exchangeRateProvider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

        try
        {
            var rates = await exchangeRateProvider.GetExchangeRates(currencies);

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

        Console.ReadLine();
    }

    private static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<ExchangeRateProvider>()
            .AddHttpClient<ExchangeRateService>();

        return serviceCollection.BuildServiceProvider();
    }
}
