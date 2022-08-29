using ExchangeRateUpdater.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Helpers
{
    public static class CurrencyHelper
    {
        public static IEnumerable<Currency> GetCNBCurrencies()
        {             
           var currencies = Enum.GetValues<CurrencyEnum>();
           return currencies.Select(currency => new Currency(Enum.GetName(currency)));
        }
    }
}