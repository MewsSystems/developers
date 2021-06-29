using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Handle exchange rate from Czech National bank
    /// See https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
    /// </summary>
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const char Separator = '|';
        private const int AmountIndex = 2;
        private const int CodeIndex = 3;
        private const int RateIndex = 4;
        
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CnbExchangeRateProvider(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies is null || !currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }
            
            string apiUrl = _configuration.GetAppSettingValue("CnbApiUrl");
            string responseString = await _httpClient.GetStringAsync(apiUrl);
            
            if (string.IsNullOrEmpty(responseString))
            {
                return Enumerable.Empty<ExchangeRate>();
            }
            
            int lineIndex = 0;
            var exchangeRates = new List<ExchangeRate>();

            using var reader = new StringReader(responseString);
            for (var line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
            {
                switch (lineIndex)
                {
                    case 0: // The first line of the text consists of the date for which the exchange rate was declared in DD.MM.YYYY format.
                    case 1: // This second line consists of a header in the following form: Country|Currency|Amount|Code|Rate
                        break;
                    default:
                        var exchangeRate = GetExchangeRate(line, currencies);
                        if (exchangeRate is not null)
                        {
                            exchangeRates.Add(exchangeRate);
                        }
                        break;
                }
                    
                lineIndex++;
            }

            return exchangeRates;
        }

        private static ExchangeRate GetExchangeRate(string line, IEnumerable<Currency> currencies)
        {
            string[] fields = line.Split(Separator);
            string currencyCode = fields[CodeIndex];

            var sourceCurrency = currencies.FirstOrDefault(currency => currency.Code == currencyCode);
            if (sourceCurrency is null)
            {
                return null;
            }

            if (!Int32.TryParse(fields[AmountIndex], out int amount))
            {
                return null;
            }

            if (!Decimal.TryParse(fields[RateIndex], out decimal rate) )
            {
                return null;
            }

            decimal exchangeRate = rate / amount;
            
            return new ExchangeRate(sourceCurrency, GetTargetCurrency(), exchangeRate);
        }

        private static Currency GetTargetCurrency() => new Currency("CZK");
    }
}
