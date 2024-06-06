using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater
{
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

        public static async Task Main(string[] args)
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                string bankURL = configuration.GetSection("centralBankUrl").Value;

                if (string.IsNullOrWhiteSpace(bankURL))
                {
                    throw new Exception("Bank URL is not set in configuration file.");
                }


                string serverResponse = await new RemoteData(bankURL).GetRatesForToday();

                if (string.IsNullOrWhiteSpace(serverResponse))
                {
                    Console.WriteLine("No exchange rates found.");
                }

                IEnumerable<ExchangeRate> ratesFromBank = new GetExchangeRate(serverResponse).ParseBankResponse();

                var provider = new ExchangeRateProvider(ratesFromBank);
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
