using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record CurrencyListRecord
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public CurrencyListRecord(IEnumerable<Currency> currencies)
        {
            Currencies=currencies;
        }
    }
}
