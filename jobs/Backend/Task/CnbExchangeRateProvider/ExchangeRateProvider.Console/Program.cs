using ExchangeRateProvider.Application;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Console
{
    public class Program
    {
        private static readonly IEnumerable<Currency> s_currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("JPY"),
            new Currency("GBP"),
            new Currency("AUD"),
            new Currency("CAD"),
            new Currency("CHF"),
            new Currency("CNY"),
            new Currency("SEK"),
            new Currency("NZD"),
            new Currency("MXN"),
            new Currency("SGD"),
            new Currency("HKD"),
            new Currency("NOK"),
        ];

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var mediator = services.GetRequiredService<IMediator>();

                    logger.LogInformation("Starting Exchange Rate Provider console application.");

                    var exchangeRates = await mediator.Send(new GetExchangeRatesQuery(s_currencies, new Currency("CZK")));

                    System.Console.WriteLine("Successfully retrieved exchange rates:");
                    foreach (var rate in exchangeRates)
                    {
                        System.Console.WriteLine($"{rate.SourceCurrency.Code} -> {rate.TargetCurrency.Code}: {rate.Value:F4}");
                    }

                    System.Console.WriteLine($"\nTotal rates retrieved: {exchangeRates.Count()}");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while updating exchange rates.");
                    System.Console.WriteLine($"\nAn error occurred: {ex.Message}");
                }
            }

            System.Console.WriteLine("\nPress Enter to exit.");
            System.Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configuration.AddEnvironmentVariables();
                    if (args != null)
                    {
                        configuration.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Add logging
                    services.AddLogging(configure => configure.AddConsole());

                    // Register application and infrastructure services
                    services.AddApplicationServices();
                    services.AddInfrastructureServices(hostContext.Configuration);
                });
    }
}
