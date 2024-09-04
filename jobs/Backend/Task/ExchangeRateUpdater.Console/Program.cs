using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastucture;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"), new Currency("EUR"), new Currency("CZK"), new Currency("JPY"), new Currency("KES"),
        new Currency("RUB"), new Currency("THB"), new Currency("TRY"), new Currency("XYZ")
    };

    public static async Task Main(string[] args)
    {
        // Set up a DI container
        var serviceCollection = new ServiceCollection().AddTransient<ApiClient>()
            .AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        serviceCollection.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();

        try
        {
            // Resolve the service and call the method
            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            var rates = await provider.GetExchangeRates(currencies).ToListAsync();

            System.Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates) System.Console.WriteLine(rate.ToString());
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        System.Console.ReadLine();
    }
}