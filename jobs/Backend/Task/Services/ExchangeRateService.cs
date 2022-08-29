using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<CNBExchangeRateItem>> GetDailyExhangeRate();
    }

    public class ExchangeRateService: IExchangeRateService
    {
        private readonly ICzechNationalBankClient _cnbClient;
        private readonly ICNBExchangeRateParser _csvParser;

        public ExchangeRateService(ICzechNationalBankClient client, ICNBExchangeRateParser parser)
        {
            _cnbClient = client;
            _csvParser = parser;
        }

        public async Task<IEnumerable<CNBExchangeRateItem>> GetDailyExhangeRate()
        {
            try
            {
                var response = await _cnbClient.GetExchangeRates();
                var rates = await response.Content.ReadAsStreamAsync();//using ReadAsStream because under the hood httpClient uses MemoryStream.
                return _csvParser.ParseCNBResponse(rates);
            }
            catch
            {
                return new List<CNBExchangeRateItem>();
            }          
        }
    }
}
