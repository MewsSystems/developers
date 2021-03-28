using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string TODAY_EXCHANGE_RATE_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const string DEFAULT_SOURCE_EXCHANGE_RATE_CODE = "CZK";

        private const int HEADER_LINES_COUNT = 2;
        private const int AMOUNT_INDEX = 2;
        private const int RATE_CODE_INDEX = 3;
        private const int EXCHANGE_RATE_INDEX = 4;

        private IFileDownloadService _fileDownloadService;
        private Regex _exchangeFileRegex;

        public ExchangeRateProvider(IFileDownloadService fileDownloadService)
        {
            _fileDownloadService = fileDownloadService;
            _exchangeFileRegex = new Regex(@"^\d{2}\.\d{2}\.\d{4} #\d+\nzemě\|měna\|množství\|kód\|kurz\n\w+\|\w+\|\d+\|\w+\|\d+(,\d+){0,1}");
        }


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRateFileContent = _fileDownloadService.DownloadFileContent(TODAY_EXCHANGE_RATE_URL);

            if (!_exchangeFileRegex.IsMatch(exchangeRateFileContent))
            {
                throw new Exception("Exchange rate file is in invalid format.");
            }

            var currencyCodesFlat = currencies.Select(currency => currency.Code);
            var desiredCurrencies = new HashSet<string>(currencyCodesFlat);

            var linesInExRateFile = exchangeRateFileContent.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var exchangeRates = new List<ExchangeRate>();

            foreach (var line in linesInExRateFile.Skip(HEADER_LINES_COUNT))
            {
                var colValues = line.Split('|');

                var rateCode = colValues[RATE_CODE_INDEX];
                var amount = colValues[AMOUNT_INDEX];
                var rate = decimal.Parse(colValues[EXCHANGE_RATE_INDEX].Replace(',', '.'));
                rate = rate / int.Parse(amount);

                if (desiredCurrencies.Contains(rateCode))
                {
                    var sourceCurrency = new Currency(DEFAULT_SOURCE_EXCHANGE_RATE_CODE);
                    var targetCurrency = new Currency(rateCode);

                    exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate));
                }
            }

            return exchangeRates;
        }
    }
}
