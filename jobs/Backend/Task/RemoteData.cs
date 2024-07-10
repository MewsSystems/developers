using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class RemoteData
    {
        private readonly string _bankURL;

        private HttpClient client = null;

        private string BankUrl { get => _bankURL; }

        private HttpClient BankHttpClient { get => client; }
        public RemoteData(string bankURL)
        {
            _bankURL = bankURL;

            if (string.IsNullOrWhiteSpace(bankURL))
            {
                throw new Exception("Bank URL cannot be empty or null");
            }

            client = new HttpClient();
            client.BaseAddress = new Uri(BankUrl);
        }


        public async Task<string> GetRatesForToday()
        {
            return await GetRates(DateTime.UtcNow);
        }


        // if the parameter is set we can get the exchange rate for that date
        public async Task<string> GetRates(DateTime forDate)
        {
            // set the date for the request to current if the "forDate" is in the future
            if (forDate > DateTime.UtcNow)
            {
                forDate = DateTime.UtcNow;
            }

            try
            {
                string dateForUrl = $"?date={forDate.ToString("dd.MM.yyyy")}";
                var result = await BankHttpClient.GetAsync(dateForUrl);
                result.EnsureSuccessStatusCode();

                string data = await result.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Could not get data from the bank: '{exc.Message}'.");
                throw;
            }
            finally
            {
                BankHttpClient.Dispose();
            }
        }
    }
}
