using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Core.Providers;

namespace ExchangeRateUpdater.Console
{
    public class Worker : BackgroundService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

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

        public Worker(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    System.Console.WriteLine("The process has been cancelled at {DateTime}", DateTime.UtcNow);
                    stoppingToken.ThrowIfCancellationRequested();
                }

                try
                {
                    var rates = await _exchangeRateProvider.GetExchangeRates(currencies);

                    System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        System.Console.WriteLine(rate.ToString());
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("There was an unhandled error in the worker with exception: {Exception}", ex.Message);
            }
        }
    }
}
