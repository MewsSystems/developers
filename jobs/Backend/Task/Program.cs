using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private const string CNB_URL  = "https://www.cnb.cz/";
        

        public static async Task Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                services.AddHttpClient<ExchangeRateRepository>(x =>
                    x.BaseAddress = new Uri(CNB_URL));
                services.AddSingleton<AppRoot>();
                services.AddSingleton<ExchangeRateProvider>();
                var serviceProvider = services.BuildServiceProvider();
                
                await serviceProvider.GetRequiredService<AppRoot>().RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                throw;
            }

            Console.ReadLine();
        }
    }
}
