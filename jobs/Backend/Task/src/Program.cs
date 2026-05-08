using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly Currency[] Currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("INR")
    };

    public static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddConfiguration(configuration.GetSection("Logging"));
        });

        services.ConfigureServices(configuration);

        using var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

        try
        {
            var rates = await provider.GetExchangeRatesAsync(Currencies);
            var ratesList = rates.ToList();

            if (ratesList.Count == 0)
            {
                Console.WriteLine("No exchange rates were retrieved.");
                return;
            }

            foreach (var rate in ratesList)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
        }

        Console.ReadLine();
    }
}
