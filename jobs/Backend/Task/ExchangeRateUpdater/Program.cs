using ExchangeRateUpdater.DependencyInjection.CnbApi;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.ExchangeRates;
using ExchangeRateUpdater.Model.ExchangeRates;
using ExchangeRateUpdater.Services.ExchangeRates;

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
        try
        {
            // Usually a DI container would construct all this.
            var cnbApiClient = CnbApiClientInjector.GetCnbApiClient("https://api.cnb.cz");
            var exchangeRateMapper = new ExchangeRateMapper();
            var timeService = new TimeService();
            
            var exchangeRateDataSource = new ExchangeRateDataSource(cnbApiClient, exchangeRateMapper, timeService);
            var provider = new ExchangeRateProvider(exchangeRateDataSource);
            
            var rates = await provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

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