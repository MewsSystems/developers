using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Extensions
{
    public static class ConverterExtension
    {
        public static IEnumerable<ExchangeRate> ToExchangeRates(this  ExchangeRatesDTO exchangeRates)
        {
            return exchangeRates.Rates.Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate / r.Amount));
        }
    }
}
