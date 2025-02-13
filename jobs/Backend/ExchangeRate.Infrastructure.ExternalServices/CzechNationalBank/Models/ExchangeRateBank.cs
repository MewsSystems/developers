using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models
{
    public class ExchangeRateBank
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
