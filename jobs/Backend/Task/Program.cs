using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Build the service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the ExchangeRateService from the service provider
            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

            try
            {
                await exchangeRateService.ExecuteAsync();
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ExchangeRateService>>();
                logger.LogError(ex, "An error occurred while executing the program");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = LoadConfiguration();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
            });

            services.AddHttpClient();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IHttpClientService, HttpClientService>();
            services.AddTransient<ICurrencyLoader, CurrencyLoader>();
            services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddTransient<IApiExchangeRateProvider, ApiExchangeRateProvider>();
            services.AddTransient<ITextExchangeRateProvider, TextExchangeRateProvider>();
            services.AddSingleton<CurrencyValidator>();
            services.AddSingleton<IExchangeRateParser, ExchangeRateParser>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddSingleton<IOutputService, ConsoleOutputService>();
            services.AddSingleton(configuration);
        }





        private static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
