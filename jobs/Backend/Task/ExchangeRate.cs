﻿using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {

        public string CurrencyCode { get; set; }
        public decimal CurrencyValue { get; set; }
        public override string ToString()
        {
            return $"{CurrencyCode}={CurrencyValue}";
        }
    }
}
