using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Clients
{
    internal class CzechNationalBankClient : IBankClient
    {
        private static readonly string TargetCurrencyCode = "CZK";
        private readonly HttpClient httpClient;

        public CzechNationalBankClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            var targetCurrenty = new Currency(TargetCurrencyCode);
            var exchangeRates = new List<ExchangeRate>();

            var exchangeRateTextFile = await httpClient.GetStringAsync("");
            string[] lines = exchangeRateTextFile.Split('\n');

            for (int i = 2; i < lines.Length - 1; i++)
            {
                string[] line = lines[i].Split('|');
                int amount = int.Parse(line[2]);
                var sourceCurrency = new Currency(line[3]);
                decimal rate = decimal.Parse(line[4]);
                ExchangeRate exchangeRate = new ExchangeRate(sourceCurrency, targetCurrenty, rate / amount);

                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }
    }
}
