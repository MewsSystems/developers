using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Core.Client.CzechNationalBank
{
    public class CzechNationalBankClient : BaseClient, IClient
    {
        public CzechNationalBankClient(ILogger<CzechNationalBankClient> logger, IConfiguration configuration, IHttpWrapper httpWrapper) : base(logger, configuration, httpWrapper)
        {

        }
        public async Task<IEnumerable<ExchangeRateItem>> GetExchangeRates()
        {
            // pull source from config
            // TODO: Null handling for config value            
            var rateSource = _configuration.GetValue<string>("CzechNationalBankClient:Source");
            var responseContent = await _httpWrapper.HttpGet(rateSource);

            return ParseRates(responseContent);
        }

        /// <summary>
        /// Helper method to parse rate data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IEnumerable<ExchangeRateItem> ParseRates(string data)
        {
            var _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "|",
                HasHeaderRecord = true
            };

            // ensure input is valid, exception handling should be done by calling code
            if (data == null || string.IsNullOrWhiteSpace(data) || data.Length <= 0)
            {
                throw new ArgumentNullException(nameof(data), "String input is required");
            }

            var textReader = new StringReader(data);
            using var csv = new CsvReader(textReader, _csvConfiguration);
            // skip the first line because it isn't used
            csv.Read();
            return csv.GetRecords<ExchangeRateItem>().ToList();
        }
    }
}
