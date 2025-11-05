using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ExchangeRateDTO
    {
        public string BaseCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public int Multiplier { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValidDate { get; set; }
    }
}
