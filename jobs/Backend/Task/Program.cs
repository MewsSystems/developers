using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private const string DefaultCnbBaseUrl = "https://www.cnb.cz";

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

        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var cnbBaseUrl = configuration["CnbApi:BaseUrl"] ?? DefaultCnbBaseUrl;

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = false;
                });
                builder.AddConfiguration(configuration.GetSection("Logging"));
            });

            services.AddSingleton<IExchangeRatesSource>(sp =>
                new CnbRatesSource(cnbBaseUrl, sp.GetRequiredService<ILogger<CnbRatesSource>>()));
            services.AddTransient<ExchangeRateProvider>();

            using var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ExchangeRateUpdater");
            var summaryLogger = loggerFactory.CreateLogger("ExchangeRateUpdater.Summary");

            logger.LogDebug("Starting ExchangeRateUpdater");
            logger.LogDebug("CNB base URL: {BaseUrl}", cnbBaseUrl);
            logger.LogDebug("Requested currencies: {Currencies}", string.Join(", ", currencies.Select(c => c.Code)));

            try
            {
                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
                var rates = provider.GetExchangeRates(currencies).ToList();

                logger.LogDebug("Retrieved {Count} exchange rates", rates.Count);

                summaryLogger.LogInformation("Successfully retrieved {Count} exchange rates:", rates.Count);
                foreach (var rate in rates)
                {
                    summaryLogger.LogInformation("{Rate}", rate.ToString());
                }

                return 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to retrieve exchange rates");
                return 1;
            }
        }
    }
}
