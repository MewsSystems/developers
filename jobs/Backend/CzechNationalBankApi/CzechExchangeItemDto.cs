using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechNationalBankApi
{
    public class CzechExchangeItemDto
    {
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}
