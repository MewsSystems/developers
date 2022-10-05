using Common.Configuration;
using Core.Models;
using Core.Models.CzechNationalBank;
using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Core.Client.CzechNationalBank
{
    public class CzechNationalBankClient : BaseClient, IClient
    {
        private const string CZK_CODE = "CZK";
        private Currency targetCurrency = new Currency(CZK_CODE);

        public CzechNationalBankClient(ILogger<CzechNationalBankClient> logger, IConfigurationWrapper configurationWrapper, IHttpWrapper httpWrapper) : base(logger, configurationWrapper, httpWrapper)
        {

        }
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            // pull source from config
            var rateSource = _configurationWrapper.GetConfigValueAsString("CzechNationalBankClient:Source");
            var responseContent = await _httpWrapper.HttpGet(rateSource);

            return ParseRates(responseContent);
        }

        /// <summary>
        /// Helper method to parse rate data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IEnumerable<ExchangeRate> ParseRates(string data)
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
            var parsedRates = csv.GetRecords<CzechNationalBankExchangeRateItem>().ToList();

            // map to common object with calculated rate
            var calculatedRates = new List<ExchangeRate>();
            foreach (var parsedRateeData in parsedRates)
            {
                calculatedRates.Add(new ExchangeRate(
                    new Currency(parsedRateeData.Code),
                    targetCurrency,
                    parsedRateeData.Rate / parsedRateeData.Amount
                    ));
            }
            return calculatedRates;
        }
    }
}
