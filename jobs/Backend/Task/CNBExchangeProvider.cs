using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CNBExchangeProvider : ExchangeRateProvider
    {
        private CNBStringDataParser _parser;
        public CNBExchangeProvider(string sourceUrlBase) : base(sourceUrlBase)
        {
            _parser = new CNBStringDataParser();
        }

        public override IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string sourceUrl = $"{SourceUrlBase}?date={ DateTime.Now.ToString("dd.MM.yyyy")}";
            var stringData = GetStringDataFromSource(sourceUrl);

            return _parser.Parse(stringData.Result);
        }
    }
}
