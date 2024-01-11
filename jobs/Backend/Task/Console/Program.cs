using Microsoft.Extensions.DependencyInjection;
using ExchangeRateProvider;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.Config;

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

    public static void Main(string[] args)
    {

        var serviceProvider = new ServiceCollection()
            .AddExchangeRateProvider()
            .BuildServiceProvider();

        try
        {
            var provider = serviceProvider.GetRequiredKeyedService<IExchangeRateProvider>(Source.CzechNationalBank);
            var rates = provider.GetExchangeRates(currencies);

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
}