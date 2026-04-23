using System;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateLogger;
using ExchangeRateUpdaterConsole.src;
using ExchangeRateUpdaterModels.Models;

namespace ExchangeRateUpdaterConsole
{
    public static class Program
    {
        private static IEnumerable<CurrencyModel> currencies = CurrenciesMockModel.GetAllCurrencies().Select(code => new CurrencyModel(code));

        public static async Task Main(string[] args)
        {
            try
            {
                Logger.Configure();

                ExchangeRateProvider provider = new ExchangeRateProvider();
                IEnumerable<ExchangeRateModel> rates = await provider.GetExchangeRatesAsync(currencies);

                Log.Information($"Successfully retrieved {rates.Count()} exchange rates:");


                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Could not retrieve exchange rates");

            }

            Console.ReadLine();
        }
    }
}
