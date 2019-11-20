using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateMemorySource : IExchangeRateSource
    {
        public ExchangeRateMemorySource(params ExchangeRate[] rates)
        {
            Rates = rates;
        }

        public ExchangeRate[] Rates { get; }

        public Task<ExchangeRate[]> Load() => Task.FromResult(Rates);
    }
}
