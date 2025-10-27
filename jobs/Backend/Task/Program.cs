using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models;
using ExchangeRateUpdater.Services.Models.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddServices(builder.Configuration);
            var serviceProvider = builder.Build().Services;

            var currencies = GetCurrencies(builder.Configuration);
            GetExchangeRate(serviceProvider, currencies);
        }

        private static void GetExchangeRate(
            IServiceProvider serviceProvider,
            IEnumerable<Currency> currencies)
        {
            try
            {
                var apiClient = serviceProvider.GetRequiredService<IApiClient<CnbRate>>();
                var logger = serviceProvider.GetService<ILogger<ExchangeRateProvider>>();
                var dateTimeSource = serviceProvider.GetService<IDateTimeSource>();

                var provider = new ExchangeRateProvider(
                    apiClient,
                    logger!);

                var rates = provider.GetExchangeRates(currencies).Result;

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

        private static IEnumerable<Currency> GetCurrencies(IConfiguration configuration)
        {
            var section = configuration
                .GetSection(Constants.ExchangeRateConfiguration)
                .Get<ExchangeRateConfiguration>();

            return section!.CurrencyCodes.Select(code => new Currency(code));
        }
    }
}
