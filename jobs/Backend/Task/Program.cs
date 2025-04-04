using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater;

public class Program
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

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

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
            logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Add logging to log to the console
        serviceCollection.AddLogging(configure => configure.AddConsole());

        serviceCollection
            .AddSingleton<ExchangeRateProvider>()
            .AddHttpClient<IExchangeRateService, ExchangeRateService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30); // 30 sec timeout
            })
            .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));

        return serviceCollection.BuildServiceProvider();
    }
}
