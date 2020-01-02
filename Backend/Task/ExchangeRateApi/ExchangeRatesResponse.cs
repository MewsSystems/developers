using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateApi
{
    /// <summary>
    /// Class for storing data obtained from API
    /// </summary>
    public class ExchangeRatesResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
