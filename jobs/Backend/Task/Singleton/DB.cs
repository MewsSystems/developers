using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Singleton
{
    internal sealed class DB
    {
        private static DB? _instance;

        private Dictionary<string, Rate> _rates;

        private DB() => _rates = new();

        public static DB GetInstance() => _instance ??= new DB();

        public void Add(string key, Rate value) => _rates.Add(key, value);

        public bool TryGetValue(string key, out Rate rate) => _rates.TryGetValue(key, out rate);

        public bool IsEmpty() => !_rates.Any();
    }
}
