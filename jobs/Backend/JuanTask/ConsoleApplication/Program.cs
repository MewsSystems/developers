using Application;
using Application.ExchangeRateProvider;
using Domain.Entities;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

public static class Program
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

        try
        {

            IServiceProvider serviceProvider = BuildServiceProvider();

            IExchangeRateProvider provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            IEnumerable<ExchangeRate> rates = await provider.GetExchangeRates(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

            foreach (ExchangeRate rate in rates)
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

    private static IServiceProvider BuildServiceProvider()
    {

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", false, true);

        IConfiguration configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();

        IServiceCollection services = new ServiceCollection()
            .AddSerilog()
            .AddApplication()
            .AddInfrastructure(configuration);

        return services.BuildServiceProvider();

    }
}