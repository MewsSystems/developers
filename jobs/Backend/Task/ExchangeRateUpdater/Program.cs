using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public static class Program
    {
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

        static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ExchangeRateClientConfig>()
                .Configure(opt=> configuration.GetSection("ExchangeRateClient").Bind(opt));
            
            services.AddSingleton<ExchangeRateProvider>();
            services.AddSingleton<IExchangeRateParser, ExchangeRateParser>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddHttpClient<ICnbExchangeRateHttpClient, CnbExchangeRateHttpClient>(
                (serviceProvider, httpClient) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<ExchangeRateClientConfig>>();

                    if (!Uri.TryCreate(config.Value.BaseUrl, UriKind.Absolute, out var baseUri))
                        throw new Exception("Invalid BaseUrl");
                    
                    httpClient.BaseAddress = baseUri;
                    httpClient.Timeout = TimeSpan.FromMilliseconds(config.Value.TimeoutMilliseconds);
                });
        }

        public static async Task Main(string[] args)
        {
            try
            {
                var configuration = BuildConfiguration();
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection, configuration);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
                var rates = (await provider.GetExchangeRates(currencies)).ToList();

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (var rate in rates)
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
    }
}
