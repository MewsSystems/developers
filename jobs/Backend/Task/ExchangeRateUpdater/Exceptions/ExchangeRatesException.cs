using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Exceptions
{
    public class ExchangeRatesException : Exception
    {
        public bool Retriable { get; }
        public ExchangeRatesException(string message, bool retry) : base(message) 
        {
            Retriable = retry;
        }
    }
}
