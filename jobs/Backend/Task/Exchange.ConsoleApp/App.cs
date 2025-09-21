using Exchange.Application.Services;
using Exchange.Domain.ValueObjects;

namespace Exchange.ConsoleApp;

public class App(IExchangeRateProvider exchangeRateProvider)
{
    private static readonly IEnumerable<Currency> Currencies =
    [
        Currency.USD,
        Currency.BRL,
        Currency.EUR,
        Currency.CZK,
        Currency.JPY,
        Currency.KES,
        Currency.RUB,
        Currency.THB,
        Currency.TRY,
        Currency.IDR
    ];

    public async Task RunAsync()
    {
        try
        {
            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

            Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");

            foreach (var exchangeRate in exchangeRates)
            {
                Console.WriteLine(exchangeRate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}