using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Helpers;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var currencies = CurrencyHelper.GenerateCurrencies(
                "CZK", "AUD", "BRL", "BGN", "CAD", "CNY", "DKK", "EUR", "HKD", "HUF", "ISK", "XDR", "INR",
                "IDR", "ILS", "JPY", "MYR", "MXN", "NZD", "NOK", "PHP", "PLN", "RON", "SGD", "ZAR", "KRW",
                "SEK", "CHF", "THB", "TRY", "GBP", "USD");

            var host = ServicesInstaller.InstallServices();

            try
            {
                var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (ExchangeRate rate in rates)
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
