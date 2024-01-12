using Microsoft.Extensions.DependencyInjection;
using ExchangeRateProvider;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.Config;
using ExchangeRateProvider.Exceptions;

public static class Program
{
    private static readonly ICollection<Currency> currencies =
    [
        new("USD"),
        new("EUR"),
        new("CZK"),
        new("JPY"),
        new("KES"),
        new("RUB"),
        new("THB"),
        new("TRY"),
        new("XYZ")
    ];

    public static async Task Main(string[] args)
    {

        var serviceProvider = new ServiceCollection()
            .AddExchangeRateProvider()
            .BuildServiceProvider();

        try
        {
            var provider = serviceProvider.GetRequiredKeyedService<IExchangeRateProvider>(Source.CzechNationalBank);

            var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);
            var rates = await provider.GetExchangeRates(currencies, date);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (UnexpectedException e)
        {
            Console.WriteLine($"Could not retrieve exchange rates. UnexpectedException: {e.Message} / CustomData: {e.DataToString()}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}