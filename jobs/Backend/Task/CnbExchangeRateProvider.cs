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
                        var exchangeRate = GetExchangeRate(line);
                        if (exchangeRate is not null)
                        {
                            exchangeRates.Add(GetExchangeRate(line));
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
                    case "Code":
                        _codeIndex = i;
                        break;
                    case "Rate":
                        _rateIndex = i;
                        break;
                }
            }
        }

        private bool IsFieldIndexValid() => _codeIndex != -1 && _rateIndex != -1;

        private ExchangeRate GetExchangeRate(string line)
        {
            string[] fields = line.Split(Separator);
            var targetCurrency = new Currency(fields[_codeIndex]);

            return Decimal.TryParse(fields[_rateIndex], out var value) 
                ? new ExchangeRate(GetSourceCurrency(), targetCurrency, value) 
                : null;
        }

        private static Currency GetSourceCurrency() => new Currency("CZK");
    }
}
