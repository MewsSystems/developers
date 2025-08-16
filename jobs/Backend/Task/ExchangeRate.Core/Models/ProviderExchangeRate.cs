using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Models
{
    public class ProviderExchangeRate
    {
        public ProviderExchangeRate(string currencyCode, decimal value)
        {
            ThreeLetterISOCurrencyCode = currencyCode;
            Rate = value;
        }

        public string ThreeLetterISOCurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }
}
