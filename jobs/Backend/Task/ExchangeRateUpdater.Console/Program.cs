using Infrastructure.Models.AppSettings;
using Infrastructure.Models.Exceptions;
using Infrastructure.Models.Responses;
using Infrastructure.Services.Abstract;
using Infrastructure.Services.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

        public static async Task Main(string[] args)
        {
            try
            {
                var builder = Host.CreateApplicationBuilder(args);
                builder.Services.Configure<CzechNationalBankSettings>(builder.Configuration.GetSection("CzechNationalBankSettings"));
                builder.Services.LoadDependencyInjection();

                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();

                using IHost host = builder.Build();

                using var serviceScope = host.Services.CreateScope();
                var exchangeRateProvider = serviceScope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();

                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

                if (rates == null)
                {
                    Console.WriteLine("There were no rates to retrieve.");
                }

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
