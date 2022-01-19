using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LanguageExt;

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
            new Currency("HUF")
        };
        
        public static void Main(string[] args)
        {
            try
            {
                using var httpClient = new HttpClient();
                
                var provider = new ExchangeRateProvider(c =>
                {
                    var cnbData = CnbFunctions.DownloadCnbCurrencyData(
                            CnbFunctions.LoadCnbFileContent(
                                httpClient,
                                "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt"),
                            error =>
                            {
                                Console.WriteLine(error.ToString());
                                return Unit.Default;
                            })
                        .Result;

                    return CnbFunctions.ConvertCnbData(cnbData, c);
                });

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
