using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private const string CnbUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
        private readonly Currency BaseCurrency = new Currency("CZK");
        private class CnbExchangeRate // mapping
        {
            public string Country { get; set; }
            public string Currency { get; set; }
            public string Code { get; set; }
            public int Amount { get; set; }
            public decimal Rate { get; set; }
        }
        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ExchangeRate?>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies)
        {
            var response = await _httpClient.GetStringAsync(CnbUrl);

            var linesOfRates = response.Split(new[] { "\n" }, StringSplitOptions.None); // lines of extracted rates from CNB

            var rates = new List<ExchangeRate>();

            foreach (var line in linesOfRates.Skip(2)) // skip the header
            {
                try
                {
                    var columns = line.Split('|');
                    var cnbRate = new CnbExchangeRate
                    {
                        Country  = columns[0].Trim(),
                        Currency = columns[1].Trim(),
                        Amount   = int.Parse(columns[2].Trim(), CultureInfo.InvariantCulture),
                        Code     = columns[3].Trim(),
                        Rate     = decimal.Parse(columns[4].Trim(), CultureInfo.InvariantCulture)
                    };

                    var sourceCurrency = new Currency(cnbRate.Code);

                    if (requestedCurrencies.Any(c => c.Code.ToLower() == sourceCurrency.Code.ToLower()))
                    {
                        rates.Add(new ExchangeRate(BaseCurrency, sourceCurrency, cnbRate.Rate, cnbRate.Amount));
                    }
                }
                catch (FormatException fe)
                {
                    Console.WriteLine($"Formatting error in the line '{line}': {fe.Message}");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine($"Data missing in the line: {line}");
                }
            }

            return rates;
        }
    }
}
