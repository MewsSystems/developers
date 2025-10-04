using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Decorator
{
    internal abstract class LoadRates : ILoadRates
    {
        protected ILoadRates wrapper;

        protected LoadRates(ILoadRates wrapper) => this.wrapper = wrapper;

        public abstract Task<bool> Load(string data);
    }
}