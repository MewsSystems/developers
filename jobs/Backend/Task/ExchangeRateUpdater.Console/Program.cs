using Adapter.Http.CnbApi.Repository;
using Adapter.Http.CnbApi.Settings;
using ExchangeRateUpdater.Console.Configuration;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace ExchangeRateUpdater.Console
{


    public static class Program
    {
        const string ApplicationName = "CNB Exchange Rate";
        private static Logger _logger;

        public static async Task Main(string[] args)
        {
            try
            {
                var iHost = Startup(args);

                _logger.Information("Starting CurrencyCode Exchange");

                using var scope = iHost.Services.CreateScope();
                var useCase = scope.ServiceProvider.GetRequiredService<GetExchangeRatesUseCase>();
                var appSettings = scope.ServiceProvider.GetRequiredService<ExchangeRateSettings>();
                var currencies = appSettings.Currencies.Select(currency => new CurrencyCode(currency));

                _logger.Information($"Found {currencies.Count()} Currencies");
                _logger.Information($"Execute CurrencyCode Exchange w/ default CurrencyCode {appSettings.DefaultCurrency}. Currencies {{{{string.Join(',', currencies)}}}} ");

                var rates = await useCase.Execute(appSettings.DefaultCurrency, currencies);

                _logger.Information($"Received {rates.Count()} Valid CurrencyCode Exchange");

                System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                    System.Console.WriteLine(rate.ToString());
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }

        private static IHost Startup(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var configurationRoot = context.Configuration;

                    var settings = configurationRoot.GetSection(nameof(ExchangeRateSettings)).Get<ExchangeRateSettings>();
                    services.AddTransient<IExchangeRateRepository, CnbExchangeRateRepository>();
                    services.AddSingleton(typeof(GetExchangeRatesUseCase));
                    services.AddSingleton(settings);

                    var configurationSettings = configurationRoot.GetSection(nameof(ConfigurationSettings)).Get<ConfigurationSettings>();
                    services.AddSingleton(settings);

                    _logger = SerilogConfiguration.Create(ApplicationName, configurationSettings);
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog(_logger, true);
                    });
                    _logger.Information("Logger created.");
                }).Build();
        }
    }
