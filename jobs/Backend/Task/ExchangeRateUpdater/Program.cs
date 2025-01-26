using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using ExchangeRateUpdater.Sources;
using ExchangeRateUpdater.Sources.CzechNationalBank;

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
            new Currency("XYZ"),
            new Currency("AUD")
        };
        private static IRateSource CzechNationalBankSource = new CzechNationalBankRateSource(new CzechNationalBankRateParser(), new HttpClient());

        public async static Task Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider([CzechNationalBankSource]);
                var czkCurrency = new Currency("CZK");
                var rates = await provider.GetLatestExchangeRates(czkCurrency, currencies).ToListAsync();

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
        }
    }
}
