using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class CnbExchangeRateService : ICnbExchangeRateService
    {
        private readonly CnbExchangeRateFixingConfiguration _configuration;                
        private readonly ILogger _logger;                

        public CnbExchangeRateService(
            CnbExchangeRateFixingConfiguration configuration,
            ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            HashSet<string> currencyCodes = currencies.ToList().Select(x => x.Code).ToHashSet();            

            List<ExchangeRate> rates = new List<ExchangeRate>();
            using (HttpClient httpClient = new HttpClient())
            {
                var getCurrencyListFunc = new Func<string, Task<string>>(url =>
                {
                    return RetryHelper.RetryOnExceptionAsync(() => httpClient.GetStringAsync(url),
                        _configuration.RetryAttempts,
                        TimeSpan.FromMilliseconds(_configuration.RetryTimeout),
                        _logger);
                });

                rates.AddRange(ProcessList(await getCurrencyListFunc(_configuration.MainCurrenciesListUrl), currencyCodes));
                rates.AddRange(ProcessList(await getCurrencyListFunc(_configuration.OtherCurrenciesListUrl), currencyCodes));
            }
            return rates;
        }

        private IEnumerable<ExchangeRate> ProcessList(string list, HashSet<string> currencyCodes)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            string[] lines = list.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = _configuration.DataStartLine; i < lines.Length; i++)
            {
                var items = lines[i].Split(_configuration.ColumnSeparator);
                if (currencyCodes.Contains(items[_configuration.CodeColumnId]))
                {
                    var exchangeRate = new ExchangeRate(
                    new Currency(_configuration.SourceCurrency),
                    new Currency(items[_configuration.CodeColumnId]),
                    Utils.Parsers.Parse(items[_configuration.RateColumnId]) / Utils.Parsers.Parse(items[_configuration.AmountColumnId]));
                    rates.Add(exchangeRate);
                }
            }
            return rates;
        }    
    }
}
