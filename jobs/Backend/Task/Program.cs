using ExchangeRateUpdater.ExchangeRateProviders;
using ExchangeRateUpdater.ExchangeRateProviders.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static ConfigurationBuilder ConfigureServices(ConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");

            return configurationBuilder;
        }

        public static ServiceCollection RegisterServices(ServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.Configure<ExchangeRateUpdaterConfiguration>(configuration.GetSection("configuration"));

            serviceCollection.RegisterCzechNationalBankProvider(configuration, GetHttpRetryPolicy());
            
            return serviceCollection;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(httpResponseMessage => httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }

        public static async Task Main(string[] args)
        {
            var configuration = ConfigureServices(new ConfigurationBuilder()).Build();
            var serviceProvider = RegisterServices(new ServiceCollection(), configuration).BuildServiceProvider();

            try
            {
                var exchangeRateUpdaterConfiguration =
                    serviceProvider.GetService<IOptions<ExchangeRateUpdaterConfiguration>>().Value;

                var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
                var getExchangeRatesResult = await exchangeRateProvider.GetExchangeRates(exchangeRateUpdaterConfiguration.CurrencyCodes);

                if (getExchangeRatesResult.HasSucceeded)
                {
                    var exchangeRates = getExchangeRatesResult.Value.ToArray();

                    Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");

                    foreach (var exchangeRate in exchangeRates)
                    {
                        Console.WriteLine(exchangeRate.ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve exchange rates: '{getExchangeRatesResult.ErrorMessage}'");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{exception.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
