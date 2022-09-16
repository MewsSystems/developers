﻿using CzechNationalBankGateway;
using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    public static class ExchangeRateCache
    {

        private static readonly TimeSpan _validitySpan = new(1,0,0);
        private static DateTime _expirationDate = DateTime.MinValue;

        private static Dictionary<Currency, ExchangeRate> _index;
        public static Dictionary<Currency, ExchangeRate> Index
        {
            get
            {
                if (_index == null || _expirationDate < DateTime.Now)
                {
                    var items = LoadExchangeRates();
                    Index = items.ToDictionary(i => i.SourceCurrency, i => i);
                }
                return _index;

            }
            private set
            {
                _index = value;
                _expirationDate = DateTime.Now.Add(_validitySpan);
            }
        }


        public static IEnumerable<ExchangeRate> LoadExchangeRates()
        {
            return Task.Run(() => ExchangeRateGatewayCNB.GetExchangeRates()).Result;
        }
    }
}
