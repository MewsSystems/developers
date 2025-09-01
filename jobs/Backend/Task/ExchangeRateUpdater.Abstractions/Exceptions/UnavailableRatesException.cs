using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions.Exceptions
{
    public class UnavailableRatesException : Exception
    {
        public UnavailableRatesException()
        { 
        }

        public UnavailableRatesException(string message)
            : base(message)
        { 
        }

        public UnavailableRatesException(string message, Exception innerException)
            : base(message, innerException)
        { 
        }
    }
}
