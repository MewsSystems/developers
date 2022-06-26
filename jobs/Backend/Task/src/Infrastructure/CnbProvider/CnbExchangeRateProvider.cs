using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using Domain;
using Application;

namespace Infrastructure.CnbProvider
{
    /// <summary>
    /// Uses the API of Ceska narodni banka (CNB) to obtain the exchange rates 
    /// </summary>
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const string CNB_API_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private static readonly Currency TARGET_CURRENCY = new Currency("CZK");
        private static readonly CsvConfiguration CSV_CONFIGURATION = new CsvConfiguration(new CultureInfo("cs-CZ"))
        {
            Delimiter = "|"
        };

        private readonly HttpClient httpClient;

        public CnbExchangeRateProvider(HttpClient httpClient) =>
            this.httpClient = httpClient;

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            HttpResponseMessage response = await httpClient.GetAsync(CNB_API_URL);
            response.EnsureSuccessStatusCode();

            IEnumerable<CnbRecord> records = await ExtractCnbRecords(response.Content);
            Dictionary<string, CnbRecord> currencyCodeMap = CreateCurrencyCodeMap(records);
            return GetExchangeRatesForCurrencies(currencies, currencyCodeMap);
        }

        private async Task<IEnumerable<CnbRecord>> ExtractCnbRecords(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();
            var reader = new StreamReader(stream);

            removeFirstLineContainingUnrelatedData(reader);
            var csvReader = new CsvReader(reader, CSV_CONFIGURATION);
            return csvReader.GetRecords<CnbRecord>();
        }

        private void removeFirstLineContainingUnrelatedData(TextReader reader)
        {
            reader.ReadLine();
        }

        private Dictionary<string, CnbRecord> CreateCurrencyCodeMap(IEnumerable<CnbRecord> records)
        {
            var recordMap = new Dictionary<string, CnbRecord>();
            foreach (var record in records)
            {
                recordMap.Add(record.Code, record);
            }
            return recordMap;
        }

        private ICollection<ExchangeRate> GetExchangeRatesForCurrencies(IEnumerable<Currency> currencies, Dictionary<string, CnbRecord> currencyCodeMap)
        {
            var exchangeRates = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                CnbRecord record;
                if (currencyCodeMap.TryGetValue(currency.Code, out record))
                {
                    exchangeRates.Add(new ExchangeRate(currency, TARGET_CURRENCY, record.Rate / record.Amount));
                }
            }
            return exchangeRates;
        }
    }
}
