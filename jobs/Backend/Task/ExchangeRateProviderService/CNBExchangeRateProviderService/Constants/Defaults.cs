using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Constants;

internal static class Defaults
{
    public readonly struct CURRENCY
    {
        public static readonly CurrencyDto BaseCurrency = new()
        {
            Code = "CZK"
        };
        public static readonly int CurrencyISOLength = 3;
    }
}
