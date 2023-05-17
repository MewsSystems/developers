using System;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            try
            {
                var provider = app.Services.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync();
                
                if (rates != null)
                {
                    // Successfully retrieved {rates.Count()} exchange rates:
                    Console.WriteLine($"Úspěšně načteno {rates.Count()} směnných kurzů:");

                    foreach (var rate in rates)
                    {
                        var country = rate.TargetCurrency.Country;
                        var codes = $"{rate.SourceCurrency}/{rate.TargetCurrency}";
                        var value = rate.Value;
                    
                        var line = $"{country,-20} -> {codes,7} = {rate.Value}";
                        Console.WriteLine(line);
                    }
                } 
                else
                {
                    Console.WriteLine($"Could not retrieve exchange rates");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                // Options
                services.AddOptions<CzechNationalBankOptions>().Bind(hostContext.Configuration.GetSection("CzechNationalBankApi"));

                // Services
                services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddSingleton<IForeignExchangeService, CzechNationalBankService>();

                // HTTP Clients
                services.AddHttpClient<IForeignExchangeService, CzechNationalBankService>(client =>
                {
                    client.BaseAddress = new Uri(hostContext.Configuration["CzechNationalBankApi:BaseAddress"]);
                });
            });

            return host;
        }
    }
}
