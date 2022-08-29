using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Startup : IHostedService
    {

        private static readonly IEnumerable<Currency> currencies = CurrencyHelper.GetCNBCurrencies();
        private readonly IExchangeRateProvider provider;

        public Startup(IExchangeRateProvider provider)
        {
             this.provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var rates = await provider.GetExchangeRates(currencies);

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

            Console.ReadLine();
         }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
