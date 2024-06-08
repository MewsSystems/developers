using Application;
using Application.CzechNationalBank.Providers;

using Autofac;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ExchangeRateUpdater
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
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

        public static void Main(string[] args)
        {
            // I did think about moving this to the Application project but this seems 
             // quite console app / test type of functionality so left it here.  In reality you 
             // might expect ExchangeRateProvider to be exposed as an API or just be available as 
             // an internal service
            try
            {
                var container = DependencyInjection.Register();
                var provider = container.Resolve<IExchangeRateProvider>();
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

            Console.ReadLine();
        }
    }
}
