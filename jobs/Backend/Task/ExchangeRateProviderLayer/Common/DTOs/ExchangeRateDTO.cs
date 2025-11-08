using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ExchangeRateDTO
    {
        public required string BaseCurrencyCode { get; set; }
        public required string TargetCurrencyCode { get; set; }
        public int Multiplier { get; set; }
        public decimal Rate { get; set; }
        public DateOnly ValidDate { get; set; }
    }
}
