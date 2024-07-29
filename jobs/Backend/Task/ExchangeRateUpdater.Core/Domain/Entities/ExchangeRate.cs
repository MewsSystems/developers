using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class ExchangeRate
    {

        public Currency SourceCurrency { get; set; }
        public decimal SourceValue { get; set; }
        public Currency TargetCurrency { get; set;  }
        public decimal TargetValue { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={TargetValue}";
        }
    }
}
