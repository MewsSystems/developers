using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Models
{
    public class CurrencyExchangeRate
    {
        public CurrencyExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }
        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal Value { get; }
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
