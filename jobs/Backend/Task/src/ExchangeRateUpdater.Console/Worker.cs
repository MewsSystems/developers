using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Core.Exceptions;

namespace ExchangeRateUpdater.Console
{
    public class Worker : BackgroundService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;


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
                    var rates = await _exchangeRateProvider.GetExchangeRates();

                    System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        System.Console.WriteLine(rate.ToString());
                    }
                }
                catch (CzechNationalBankApiException e)
                {
                    System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Error retrieving exchange rates: '{e.Message}'.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("There was an unhandled error in the worker with exception: {Exception}", ex.Message);
            }
        }
    }
}
