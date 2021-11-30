
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Entities.Xml;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Tests
{
    internal class TestCnbXmlSource:ICnbXmlSource
    {
        private CnbXmlRatesDocument document;

        public TestCnbXmlSource()
        {
            document = new CnbXmlRatesDocument();
            document.RatesTable = new CnbXmlRatesTable();

        }

        public void SetRates(IEnumerable<ExchangeRate> rates, Currency targetCurrency)
        {
            document.RatesTable.Rates = rates.Select(i => new CnbXmlRate()
            {
                CurrencyCode = i.SourceCurrency.Code, Multiplier = 1,
                ValueString = i.Value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "," })
            }).ToArray();
        }

        public Task<CnbXmlRatesDocument> GetRates()
        {
            return Task.FromResult(document);
        }
    }
}
