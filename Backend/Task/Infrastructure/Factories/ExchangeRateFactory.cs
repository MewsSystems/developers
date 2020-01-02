using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Factories
{
    public class ExchangeRateFactory
    {
        public static ExchangeRate Create(string sourceCurrencyCode, Currency targetCurrency, decimal rate)
        {
            Currency source = new Currency(sourceCurrencyCode);

            return new ExchangeRate(source, targetCurrency, rate);
        }
    }
}
