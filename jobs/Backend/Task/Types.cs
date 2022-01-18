using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace ExchangeRateUpdater
{
    public static class Types
    {
        public record CnbCurrencyData(int Amount, Currency Currency, decimal Value)
        {
            public decimal CalculateRateToOneCzk() => Value / Amount;
        }
    }
}