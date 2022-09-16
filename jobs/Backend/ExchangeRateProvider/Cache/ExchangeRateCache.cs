using CzechNationalBankGateway;
using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Cache
{
    public class ExchangeRateCache : ICache<Currency, ExchangeRate>
    {
        private static TimeSpan _validitySpan = new(1, 0, 0);
        private DateTime _expirationDate = DateTime.MinValue;
        private IExchangeRateGatewayCNB _exchangeRateGatewayCNB;

        public ExchangeRateCache(IExchangeRateGatewayCNB exchangeRateGatewayCNB)
        {
            _exchangeRateGatewayCNB = exchangeRateGatewayCNB;
        }

        private Dictionary<Currency, ExchangeRate> _index;
        public Dictionary<Currency, ExchangeRate> Index
        {
            get
            {
                if (_index == null || _expirationDate < DateTime.Now)
                {
                    var items = LoadExchangeRates();
                    Index = items.Distinct().ToDictionary(i => i.SourceCurrency, i => i);
                }
                return _index;

            }
            private set
            {
                _index = value;
                _expirationDate = DateTime.Now.Add(_validitySpan);
            }
        }

        private IEnumerable<ExchangeRate> LoadExchangeRates()
        {
            return Task.Run(() => _exchangeRateGatewayCNB.GetExchangeRates()).Result;
        }

        public void SetValiditySpan(TimeSpan validitySpan)
        {
            _validitySpan = validitySpan;
        }
    }
}
