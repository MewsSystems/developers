using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private const string exitCmd = "exit";
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
           // changeLang("cs"); changeLang("en"); only for test purpose
            string cmd = string.Empty;
            do
            {
                Console.Clear();
                try
                {
                    var provider = new ExchangeRateProvider();
                    var rates = provider.GetExchangeRates(currencies);

                    int c = 0; 
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                        Task.WaitAny(Task.Delay(77)); // forced delay - simulates large task!
                        c++;
                    }
                    Console.WriteLine($"{Res.SuccessfullyRetrieved} {c} {Res.exchangeRates}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{Res.ErrorWhileRetrievingER }{Environment.NewLine}Details: " + e.Message);
                }

                Console.WriteLine($"{Environment.NewLine}{Res.PressAnyKeyOr} '{exitCmd}' {Res.ForClosingApp}...");
                cmd = Console.ReadLine();
            }
            while (string.Compare(cmd, exitCmd)!=0);
        }

        /// <summary>
        /// Only for test purpose!!!
        /// </summary>
        private static void changeLang(string lang)  //ONLY FOR TEST
        {
            try
            {
                var ci = new CultureInfo(lang);
                CultureInfo.DefaultThreadCurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
            }
            catch { }
        }
    }
}
