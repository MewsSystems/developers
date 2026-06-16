using ExchangeRateUpdaterApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog.Core;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace ExchangeRateUpdaterApi
{
    public static class Program
    {
        public const string ApplicationName = "MewsChallenge - ExchangeRateUpdaterApi";

        private static IHost _host;

        public static void Main(string[] args)
        {
            ISettings settings = ExchangeRateUpdaterApiConfiguration.GetExchangeRateUpdaterApiSettings();
            
            using (_host = new ApplicationHostBuilder(args, ApplicationName, settings).BuildHost())
            {
                _host.Run();
            }
            
            /*private static IEnumerable<Currency> currencies = new[]
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
            };*/

            /*try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();*/
        }
    }
}
