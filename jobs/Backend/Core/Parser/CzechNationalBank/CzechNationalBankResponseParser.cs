using Common.Csv;
using Core.Models;
using Core.Models.CzechNationalBank;

namespace Core.Parser.CzechNationalBank
{
    public class CzechNationalBankResponseParser : IResponseParser
    {
        private const string CZK_CODE = "CZK";
        private Currency targetCurrency = new Currency(CZK_CODE);
        private ICsvWrapper _csvWrapper;

        public CzechNationalBankResponseParser(ICsvWrapper csvWrapper)
        {
            _csvWrapper = csvWrapper;
        }

        public IEnumerable<ExchangeRate> ParseResponse(string data)
        {
            // ensure input is valid, exception handling should be done by calling code
            if (data == null || string.IsNullOrWhiteSpace(data) || data.Length <= 0)
            {
                throw new ArgumentNullException(nameof(data), "String input is required");
            }
            
            // parse CSV
            var parsedRates = _csvWrapper.ParseCsv<CzechNationalBankExchangeRateItem>(data, "|", true, true);

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
