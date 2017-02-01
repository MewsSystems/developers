using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Model
{
    /// <summary>
    /// ExchangeRate data transfer
    /// </summary>
    public class ExchangeRateDto
    {
        public CurrencyDto SourceCurrency { get; set; }

        public CurrencyDto TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }
    }
}
