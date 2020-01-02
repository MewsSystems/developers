using ExchangeRateProvider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ExchangeRateProvider.RateProviders
{
    public class CZKProvider : IRateProvider
    {
        public string SourceCurrencyCode => "CZK";
        private IRateProviderSettings rateProviderSettings;
        private enum CNBCurrencyFormat
        {
            Country,
            Currency,
            Amount,
            Code,
            Rate
        }

        public CZKProvider(IRateProviderSettings rateProviderSettings)
        {
            this.rateProviderSettings = rateProviderSettings;
        }

        public IDictionary<string, decimal> GetExchangeRates(DateTime? date = null)
        {
            if (string.IsNullOrEmpty(rateProviderSettings.CZKExchangeRateProviderUrl))
            {
                throw new Exception("Exchange Rate file url was not provided");
            }

            string dateQuery = $"?date={(date ?? DateTime.Now).ToString("dd.MM.yyyy")}";

            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                string ratesResponse = client.DownloadString($"{rateProviderSettings.CZKExchangeRateProviderUrl}{dateQuery}");

                IDictionary<string, decimal> exchangeRates = ParseExchangeRates(ratesResponse);

                return exchangeRates;
            }
        }

        private IDictionary<string, decimal> ParseExchangeRates(string ratesResponse)
        {
            IEnumerable<string> currencies = ratesResponse.Split(new[] { "\n" }, StringSplitOptions.None)
                                                 .Where(s => !string.IsNullOrWhiteSpace(s))
                                                 .Skip(2);

            IDictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();

            foreach (string r in currencies)
            {
                string[] currencyData = r.Split('|');
                string currencyCode = currencyData[(int)CNBCurrencyFormat.Code];

                decimal.TryParse(currencyData[(int)CNBCurrencyFormat.Rate], out decimal currencyRate);
                decimal.TryParse(currencyData[(int)CNBCurrencyFormat.Amount], out decimal currencyAmount);

                exchangeRates[currencyCode] = currencyRate / currencyAmount;
            }

            return exchangeRates;
        }
    }
}
