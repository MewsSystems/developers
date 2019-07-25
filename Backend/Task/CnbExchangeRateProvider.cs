using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string cnbExchangeRateUrl = ConfigurationManager.AppSettings["CNBExchangeRateUrl"];
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly string targetCurrencyIsoCode = "CZK";

        public ExchangeRateProviderType Type { get; set; } = ExchangeRateProviderType.Cnb;

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var data = "";
            try { 
                data = await GetExchangeRatesSourceData();
            }
            catch
            {
                logger.Error("Cannot connect to cnb exchange rate provider");
                throw;
            }

            var exchangeRates = ParseSourceData(data);

            return exchangeRates.Where(x => currencies.Contains(x.SourceCurrency));
        }

        private IEnumerable<ExchangeRate> ParseSourceData(string data)
        {
            var exchangeRates = new List<ExchangeRate>();
            var targetCurrency = new Currency(targetCurrencyIsoCode);

            IEnumerable<string> dataRows;

            try { 
                dataRows = data.TrimEnd().Split(Environment.NewLine.ToCharArray()).Skip(2); //skip first two rows that contains date info and column headers and possible empty lines at the end
            }
            catch
            {
                logger.Error("Source data are not in correct format");
                throw;
            }

            foreach(var row in dataRows)
            {
                var columns = row.Split('|');
                if(columns.Count() != 5)
                {
                    logger.Warn("This row is not in proper format: " + row);
                    continue;
                }

                var amount = 0;
                if(!int.TryParse(columns[2],out amount))
                {
                    logger.Warn("Bad format of amount in row: " + row);
                    continue;
                }
                var currencyCode = columns[3];
                var rate = 0m;
                if (!decimal.TryParse(columns[4],NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rate))
                {
                    logger.Warn("Bad format of rate in row: " + row);
                    continue;
                }

                rate /= amount;
                exchangeRates.Add(new ExchangeRate(new Currency(currencyCode), targetCurrency, rate));
            }

            return exchangeRates;
        }

        private async Task<string> GetExchangeRatesSourceData()
        {
            using(var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(cnbExchangeRateUrl))
                    using(var streamReader = new StreamReader(stream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
        }
    }
}
