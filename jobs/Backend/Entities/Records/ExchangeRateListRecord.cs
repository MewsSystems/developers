using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Records
{
    public class ExchangeRateListRecord
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
        public ExchangeRateListRecord(IEnumerable<ExchangeRate> exchangeRates)
        {
            ExchangeRates = exchangeRates;
        }
    }
}
