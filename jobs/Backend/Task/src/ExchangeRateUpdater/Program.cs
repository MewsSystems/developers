using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {

        private static IReadOnlyCollection<Currency> currencies =
        [
            new Currency("CZK"),
            new Currency("AUD"),
            new Currency("BRL"),
            new Currency("BGN"),
            new Currency("CAD"),
            new Currency("CNY"),
            new Currency("DKK"),
            new Currency("EUR"),
            new Currency("HKD"),
            new Currency("HUF"),
            new Currency("ISK"),
            new Currency("XDR"),
            new Currency("INR"),
            new Currency("IDR"),
            new Currency("ILS"),
            new Currency("JPY"),
            new Currency("MYR"),
            new Currency("MXN"),
            new Currency("NZD"),
            new Currency("NOK"),
            new Currency("PHP"),
            new Currency("PLN"),
            new Currency("RON"),
            new Currency("SGD"),
            new Currency("ZAR"),
            new Currency("KRW"),
            new Currency("SEK"),
            new Currency("CHF"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("GBP"),
            new Currency("USD")
        ];

        public static async Task Main(string[] args)
        {
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
