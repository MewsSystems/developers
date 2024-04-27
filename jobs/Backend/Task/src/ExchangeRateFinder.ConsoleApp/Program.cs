using ExchangeRateFinder.ConsoleApp.ApiClients;
using ExchangeRateFinder.ConsoleApp.Requests.Models;
using ExchangeRateFinder.ConsoleApp.Responses.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        private const string SourceCurrencyCode = "CZK";
        private const string API_URL = "https://localhost:7210/api";

        static async Task Main(string[] args)
        {

            // Sleep for 5 seconds to ensure the API is running
            WaitForApiInitializationAsync();

            string apiUrl = ConstructApiUrl(SourceCurrencyCode, currencies);
            HttpClientService apiClient = new HttpClientService();

            try
            {
                var response = await apiClient.GetCalculatedExchangeRatesAsync(apiUrl);
                var exchangeRates = JsonConvert.DeserializeObject<List<CalculatedExchangeRate>>(response);

                foreach (var exchangeRate in exchangeRates)
                {
                    Console.WriteLine(exchangeRate.ToString());
                }
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WaitForApiInitializationAsync()
        {
            Thread.Sleep(5000);
        }

        private static string ConstructApiUrl(string sourceCurrencyCode, IEnumerable<Currency> currencies)
        {
            var currencyCodes = string.Join(",", currencies.Select(c => c.Code));
            return $"{API_URL}/exchange-rates?sourceCurrencyCode={sourceCurrencyCode}&targetCurrencyCodes={currencyCodes}";
        }
    }
}