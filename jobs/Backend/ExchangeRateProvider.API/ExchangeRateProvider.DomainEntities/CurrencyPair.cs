using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.DomainEntities
{
    public class CurrencyPair
    {
        public string validFor { get; set; }
        public int order { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public double rate { get; set; }
    }
}
