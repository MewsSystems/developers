using Exchange.Application.Abstractions.ApiClients;
using Exchange.Application.Services;
using Exchange.Domain.ValueObjects;

namespace Exchange.ConsoleApp;

public class App(ICnbApiClient cnbApiClient)
{
    private static readonly IEnumerable<Currency> Currencies =
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

    public async Task RunAsync()
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var rates = provider.GetExchangeRates(Currencies);
        
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

        // var exchangeRates = await cnbApiClient.GetExchangeRatesAsync();
        // foreach (var exchangeRate in exchangeRates)
        // {
        //     Console.WriteLine(exchangeRate);
        // }
    }
}