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

    internal class CheckBankURL
    {
        private readonly string _bankURL;

        private HttpClient client = null;

        public string BankUrl { get => _bankURL; }

        public HttpClient BankHttpClient { get => client; }
        public CheckBankURL()
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _bankURL = configuration.GetSection("centralBankUrl").Value;

                if (string.IsNullOrWhiteSpace(BankUrl))
                {
                    throw new Exception("Bank URL is not set in configuration file.");
                }

                client = new HttpClient();
                client.BaseAddress = new Uri(BankUrl);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Configuration exception: '{exc.Message}'.");
            }
        }


        public async Task<bool> VerifyURLActive()
        {
            try
            {
                // we will only make a HEAD request: fast and no body!
                // we just need to make sure that the site is up and running
                using HttpRequestMessage headRequest = new HttpRequestMessage(HttpMethod.Head, BankUrl);

                HttpResponseMessage response = await BankHttpClient.SendAsync(headRequest);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Could not verify bank URL: '{exc.Message}'.");
                return false;
            }
        }
    }
}
