using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    public class ExchangeRateCache
    {
        public ExchangeRateCache(Func<List<ExchangeRate>> loadFunc)
        {
            _loadFunc = loadFunc;
        }

        private static readonly TimeSpan _validitySpan = new(1,0,0);
        private static DateTime _expirationDate = DateTime.MinValue;
        private static Func<List<ExchangeRate>> _loadFunc;

        private static Dictionary<Currency, ExchangeRate> _index;
        public static Dictionary<Currency, ExchangeRate> Index
        {
            get
            {
                if (_index == null || _expirationDate < DateTime.Now)
                {
                    var items = _loadFunc();
                    Index = items.ToDictionary(i => i.SourceCurrency, i => i);
                }
                return _index;

            }
            private set
            {
                _index = value;
                _expirationDate = DateTime.Now.Add(_validitySpan);
            }
        };
    }
}
