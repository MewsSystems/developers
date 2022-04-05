#region using
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ExchangeRateUpdater.Service
{
    public class ExchangeHostedService : IHostedService
    {
        private static IEnumerable<Currency> _currencies = new[]
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

        private readonly IExchangeRateProvider provider;

        public ExchangeHostedService(IExchangeRateProvider _provider)
        {
            provider = _provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var result = this.provider.GetExchangeRates(_currencies);

            Console.WriteLine($"Successfully retrieved {result.Count()} exchange rates:");

            foreach (var rate in result)
            {
                Console.WriteLine(rate.ToString());
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
