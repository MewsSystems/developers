using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Parsing;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICommunicator _communicator;
        private readonly IExchangeRateParser _parser;

        public ExchangeRateProvider(ICommunicator communicator, IExchangeRateParser parser)
        {
            if(communicator == null)
            {
                throw new ArgumentNullException(nameof(communicator));
            }
            _communicator = communicator;

            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }
            _parser = parser;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
            {
                return new List<ExchangeRate>();
            }

            var data = await _communicator.GetExchangeRateData();
            var parsedData = _parser.Parse(data);

            return FilterResult(currencies, parsedData);
        }

        private static List<ExchangeRate> FilterResult(IEnumerable<Currency> currencies, IEnumerable<ExchangeRate> parsedData)
        {
            var result = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var rate = parsedData.FirstOrDefault(c => c.SourceCurrency.Equals(currency));
                if (rate != null)
                {
                    result.Add(rate);
                }
            }
            return result;
        }
    }
}
