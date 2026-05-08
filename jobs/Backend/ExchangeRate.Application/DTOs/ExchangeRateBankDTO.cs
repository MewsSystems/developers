using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRateBankDTO
    {
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}
