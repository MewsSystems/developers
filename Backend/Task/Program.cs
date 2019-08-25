using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> currencies = new[]
        {
            new Currency("USA","dollar","USD"),
            new Currency("EMU","euro","EUR"),
            new Currency("Czech Republic","koruna","CZK"),
            new Currency("Japan","yen","JPY"),
            new Currency("Kenya","shilling","KES"),
            new Currency("Russia","ruble","RUB"),
            new Currency("Thailand","baht","THB"),
            new Currency("Turkey","lira","TRY"),
            new Currency("Spare","spare","XYZ")
        };

        public static Currency defaultSourceCurrency = currencies.ElementAt(2);

        public static void Main(string[] args)
        {
            Task<string> t = GetDownload();
            Console.WriteLine("Connecting...");
            t.Wait(5000);
            ResultHandler(t.Result);

        }
        static async Task<string> GetDownload()
        {
            try
            {
                string endPointURL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(endPointURL))
                using (HttpContent content = response.Content)
                {
                    string webResult = await content.ReadAsStringAsync();
                    return webResult;
                }
            }
            catch (Exception e) {

                Console.WriteLine($"Unable to connect: '{ e.Message}'.");
                return "";
            }
        }

        private static void ResultHandler(string webResult)
        {
            try
            {
                //Dont' proceed if no data
                if ((webResult == null) || (webResult.Length < 1))
                {
                    throw new Exception("No data");
                }
                
                var provider = new ExchangeRateProvider(defaultSourceCurrency, webResult);
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
