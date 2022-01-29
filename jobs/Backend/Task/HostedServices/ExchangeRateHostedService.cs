using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.Providers.ExchangeRateProvider;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.HostedServices
{
    public class ExchangeRateHostedService : IHostedService
    {
        static IEnumerable<Currency> currencies = new[]
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
        
        readonly IExchangeRateProvider _exchangeRateProvider;
        readonly IHostApplicationLifetime _applicationLifetime;

        public ExchangeRateHostedService(
            IExchangeRateProvider exchangeRateProvider,
            IHostApplicationLifetime applicationLifetime)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
            
            GetExchangeRates();
            
            // Don't want to wait till host application life time ends in this case
            _applicationLifetime.StopApplication();
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
            
            return Task.CompletedTask;
        }

        void GetExchangeRates()
        {
            try
            {
                var rates = _exchangeRateProvider.GetExchangeRates(currencies);

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
    }
}