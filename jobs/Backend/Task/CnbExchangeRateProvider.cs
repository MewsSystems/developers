using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IDateProvider _dateProvider;

        private const string Url =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={0}";

        private const char Separator = '|';

        private int _amountIndex = -1;
        private int _codeIndex = -1;
        private int _rateIndex = -1;

        public CnbExchangeRateProvider(HttpClient httpClient, IDateProvider dateProvider)
        {
            _httpClient = httpClient;
            _dateProvider = dateProvider;
        }
        
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            string url = string.Format(Url, _dateProvider.GetCurrentDate("dd.MM.yyyy"));
            
            string responseString = await _httpClient.GetStringAsync(url);
            
            if (string.IsNullOrEmpty(responseString))
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            using var reader = new StringReader(responseString);
            int lineIndex = 0;

            var exchangeRates = new List<ExchangeRate>();
            
            for (var line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
            {
                switch (lineIndex)
                {
                    case 0:
                        // do nothing
                        break;
                    case 1:
                    {
                        PopulateFieldIndex(line);
                        if (!IsFieldIndexValid())
                        {
                            return Enumerable.Empty<ExchangeRate>();
                        }

                        break;
                    }
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

        private void PopulateFieldIndex(string line)
        {
            string[] headers = line.Split(Separator);
            for (var i = 0; i < headers.Length; i++)
            {
                switch (headers[i])
                {
                    case "Amount":
                        _amountIndex = i;
                        break;
                    case "Code":
                        _codeIndex = i;
                        break;
                    case "Rate":
                        _rateIndex = i;
                        break;
                }
            }
        }

        private bool IsFieldIndexValid() => _amountIndex != -1 && _codeIndex != -1 && _rateIndex != -1;

        private ExchangeRate GetExchangeRate(string line, IEnumerable<Currency> currencies)
        {
            string[] fields = line.Split(Separator);
            string currencyCode = fields[_codeIndex];

            var sourceCurrency = currencies.FirstOrDefault(currency => currency.Code == currencyCode);
            if (sourceCurrency is null)
            {
                return null;
            }

            if (!Int32.TryParse(fields[_amountIndex], out int amount))
            {
                return null;
            }

            if (!Decimal.TryParse(fields[_rateIndex], out decimal rate) )
            {
                return null;
            }

            decimal exchangeRate = rate / amount;
            
            return new ExchangeRate(sourceCurrency, GetTargetCurrency(), exchangeRate);
        }

        private static Currency GetTargetCurrency() => new Currency("CZK");
    }
}
