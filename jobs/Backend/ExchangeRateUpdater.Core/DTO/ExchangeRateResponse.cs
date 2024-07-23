using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO
{
    public class ExchangeRateResponse
    {
        public string SourceCurrency { get; }

        public string TargetCurrency { get; }

        public decimal Value { get; }
    }
}
