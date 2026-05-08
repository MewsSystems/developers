using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Polly;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Parsers;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Determine environment (defaults to Production if not set)
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            // Setup dependency injection container
            // The DI container automatically resolves and injects dependencies based on constructor parameters.
            // When a service is requested (e.g., ExchangeRateProvider), the container:
            // 1. Inspects the constructor to see what dependencies are needed
            // 2. Creates instances of those dependencies (ICnbHttpClient, ICnbDataParser)
            // 3. Calls the constructor with the created instances
            // This eliminates manual object creation and enables loose coupling.
            var services = new ServiceCollection();

            // Configure logging
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
            });

            // Register configuration
            var cnbSettings = configuration.GetSection("CnbApi").Get<CnbApiSettings>();
            services.Configure<CnbApiSettings>(configuration.GetSection("CnbApi"));

            // Register HttpClient with Polly retry policy
            services.AddHttpClient<ICnbHttpClient, CnbHttpClient>()
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(
                        retryCount: cnbSettings?.RetryCount ?? 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(cnbSettings?.RetryDelaySeconds ?? 2)));

            // Register services
            services.AddTransient<ICnbDataParser, CnbDataParser>();
            services.AddTransient<ExchangeRateProvider>();

            // Build service provider container
            var serviceProvider = services.BuildServiceProvider();

            // Execute the application
            var currencies = new[]
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

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ExchangeRateUpdater.Program");

            try
            {
                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

                logger.LogDebug("Starting exchange rate retrieval for {CurrencyCount} currencies", currencies.Length);

                var rates = await provider.GetExchangeRatesAsync(currencies);

                logger.LogDebug("Successfully retrieved {RateCount} exchange rates", rates.Count());

                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to retrieve exchange rates");
                Console.WriteLine("Could not retrieve exchange rates. Please check the logs for details.");
            }

            Console.ReadLine();
        }
    }
}
