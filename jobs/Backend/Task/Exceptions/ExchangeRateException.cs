using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Exceptions
{
    /// <summary>
    /// Custom exception for exchange rate operations.
    /// caller can distinguish domain errors from infrastructure errors
    /// </summary>
    public class ExchangeRateException : Exception
    {
        public ExchangeRateException(string message) : base(message) { }
        public ExchangeRateException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
