using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    public class ApiResponse
    {
        public List<Rate> rates { get; set; }

        public class Rate
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
}
