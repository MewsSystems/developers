namespace ExchangeRateUpdater
{
    using ExchangeRateUpdater.ExchangeClients;
    using ExchangeRateUpdater.Models;
    using ExchangeRateUpdater.Parsers;
    using ExchangeRateUpdater.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Program
    {
        private static readonly IEnumerable<Currency> Currencies = new[]
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
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            using var serviceProvider = ConfigureServices(environment);

            try
            {
                var exchangeRateProvider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
                var rates = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ExchangeRateProvider>>();
                logger.LogError(e, "Could not retrieve exchange rates.");
            }

            Console.ReadLine();
        }

        private static ServiceProvider ConfigureServices(string environment)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
            });

            services.Configure<ExchangeRateProviderSettings>(configuration.GetSection("ExchangeRateSettings"));

            services.AddHttpClient<IExchangeRateClient, CnbExchangeRateClient>()
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt)));

            services.AddSingleton<IExchangeRateDataParser, CnbExchangeRateDataParser>();

            services.AddTransient<ExchangeRateProvider>();

            return services.BuildServiceProvider();
        }
    }
}
