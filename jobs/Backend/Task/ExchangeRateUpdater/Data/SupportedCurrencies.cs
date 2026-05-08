using ExchangeRateUpdater.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data
{
    public static class SupportedCurrencies
    {
        public static readonly IEnumerable<Currency> All = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        public static readonly Currency DefaultBaseCurrency = new Currency("CZK");
    }
}
