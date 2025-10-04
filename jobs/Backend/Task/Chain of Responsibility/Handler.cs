using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    internal abstract class Handler : IHandler
    {
        protected IHandler? next;

        public void SetNext(Handler next) => this.next = next;

        public abstract ExchangeRate GetExchangeRate(Currency currency);
    }
}
