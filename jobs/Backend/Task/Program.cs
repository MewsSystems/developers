using ExchangeRateUpdater.ExchangeProviders;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Program
    {
        private static IConfiguration _configuration { get; set; }
        private static ServiceProvider _serviceProvider { get; set; }

        public static async Task Main()
        {
            Configure();

            var exchangeProvider = _serviceProvider.GetService<IExchangeRateProvider>();
            var currencies = CurrencyHelper.GetCurrencies(_configuration);

            var rates = await exchangeProvider.GetExchangeRates(currencies, DateTimeOffset.Now.Date);

            if (rates.IsFailed)
            {
                Console.WriteLine("Wasn't able to retrieve exchange rates. Please try again later.");
            }
            else {

                var introMessage = rates.Value.Date.Date == DateTimeOffset.Now.Date ?
                    $"Successfully retrieved exchange rates for {rates.Value.Date.Date.ToString("dd.MM.yyyy")}." :
                    $"Couldn't retrieve todays exchange rates! Instead showing for {rates.Value.Date.Date.ToString("dd.MM.yyyy")}.";

                Console.WriteLine(introMessage);
                foreach (var rate in rates.Value.Rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }

            Console.ReadLine();
        }

        private static void Configure()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            //setup our DI
            _serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                        .AddConsole();
                })
                .AddFileProviders(_configuration)
                .AddExchangeProviders(_configuration)
                .BuildServiceProvider();
        }
    }
}
