using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateService
{
    public class CurrencyData
    {
        public CurrencyData(string currencyCode, decimal value, int amount)
        {
            CurrencyCode = currencyCode;
            Value = value;
            Amount = amount;
        }

        public int Amount { get; }

        public string CurrencyCode { get; }

        public decimal Value { get; }
    }
}
