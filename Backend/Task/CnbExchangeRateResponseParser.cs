using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateResponseParser : IExchangeRateResponseParser
    {
        public IEnumerable<ExchangeRate> ParseResponse(string response, Currency targetCurrency)
        {
            if (response == null)
            {
                throw new ArgumentNullException();
            }
            var strings = response.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var headersCount = 2;
            var ratesCount = strings.Length - headersCount;


            var unfilteredRates = strings.Skip(headersCount)
                .Take(ratesCount)
                .Select(x => x.Split('|'))
                .Select(x => ParseAndInitializeExchangeRate(x.ToArray(), targetCurrency));

            return unfilteredRates;
        }

        private ExchangeRate ParseAndInitializeExchangeRate(string[] rateInfo, Currency targetCurrency)
        {
            var correctLengthOfRateInfo = 5;
            var incorrectFormatMessage = "The format of response is incorrect";

            if (rateInfo.Length != correctLengthOfRateInfo)
            {
                throw new FormatException(incorrectFormatMessage);
            }
            try
            {
                return new ExchangeRate(new Currency(rateInfo[3]),
                    targetCurrency, decimal.Parse(rateInfo[4]) / int.Parse(rateInfo[2]));
            }
            catch (IndexOutOfRangeException e)
            {
                throw new FormatException(incorrectFormatMessage, e);
            }
        }
    }
}
