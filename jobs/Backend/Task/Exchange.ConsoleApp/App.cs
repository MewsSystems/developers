using Exchange.Application.Services;
using Exchange.Domain.ValueObjects;

namespace Exchange.ConsoleApp;

public class App(IExchangeRateProvider exchangeRateProvider)
{
    private static readonly IEnumerable<Currency> InitialCurrencies =
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
        var userInput = string.Empty;

        do
        {
            await DisplayExchangeRates(
                GetCurrencies(userInput)
            );

            Console.WriteLine("Enter currency codes to retrieve exchange rates for:");
            Console.WriteLine("Ex: USD,BRL,EUR");
            Console.WriteLine("Empty value to exit.");

            userInput = Console.ReadLine();
        } while (!string.IsNullOrWhiteSpace(userInput));


        Console.ReadLine();
    }

    private async Task DisplayExchangeRates(IEnumerable<Currency> currencies)
    {
        try
        {
            var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

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
    }

    private static IEnumerable<Currency> GetCurrencies(string currencyCodes = "")
    {
        if (string.IsNullOrWhiteSpace(currencyCodes))
            return InitialCurrencies;

        var codes = currencyCodes.Trim().Split(',');

        try
        {
            return codes.Select(c => Currency.FromCode(c.Trim()));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return InitialCurrencies;
    }
}