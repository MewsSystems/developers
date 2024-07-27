using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class ExchangeRate
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
