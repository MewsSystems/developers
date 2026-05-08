using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Http;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class Program
    {
        private static readonly IEnumerable<Currency> DefaultCurrencies = new[]
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

        public static async Task<int> Main(string[] args)
        {
            try
            {
                using var host = CreateHostBuilder(args).Build();

                var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                var currencies = args.Length > 0
                    ? args.Select(c => new Currency(c.Trim().ToUpper()))
                    : DefaultCurrencies;

                logger.LogInformation("Starting exchange rate retrieval...");

                var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);
                var ratesList = rates.ToList();

                if (ratesList.Count == 0)
                {
                    logger.LogWarning("No exchange rates found for the specified currencies: {Currencies}", string.Join(", ", currencies));
                    return 0;
                }

                logger.LogInformation("Successfully retrieved {Count} exchange rates:", ratesList.Count);
                foreach (var rate in ratesList)
                {
                    Console.WriteLine(rate.ToString());
                }

                logger.LogInformation("Exchange rate retrieval completed successfully.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.Sources.Clear();

                    var appLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location)!;

                    config.SetBasePath(appLocation)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    services.Configure<CnbApiOptions>(
                        hostContext.Configuration.GetSection("CnbApi"));


                    services.AddHttpClient<ICnbApiClient, CnbApiClient>((serviceProvider, client) =>
                    {
                        var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<CnbApiOptions>>().Value;
                        client.BaseAddress = new Uri(options.BaseUrl);
                        client.Timeout = TimeSpan.FromSeconds(options.RequestTimeoutSeconds);
                    });


                    services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
                    services.AddSingleton<ICnbExchangeRateRepository, CnbExchangeRateRepository>();
                    services.AddSingleton<ICnbApiClient, CnbApiClient>();

                    services.AddLogging(configure =>
                        configure.AddConsole().AddDebug().SetMinimumLevel(LogLevel.Information));
                });
    }
}
