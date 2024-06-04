using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastucture;

namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"), new Currency("EUR"), new Currency("CZK"), new Currency("JPY"), new Currency("KES"),
        new Currency("RUB"), new Currency("THB"), new Currency("TRY"), new Currency("XYZ")
    };

    public static void Main(string[] args)
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var rates = provider.GetExchangeRates(currencies);

            System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates) System.Console.WriteLine(rate.ToString());
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        System.Console.ReadLine();
    }
}