using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Exceptions
{
    /// <summary>
    /// Represents an error while retrieving or parsing exchange rate data from an external source.
    /// </summary>
    public class ExchangeRateSourceException : Exception
    {
        public ExchangeRateSourceException(string message) : base(message)
        {
        }

        public ExchangeRateSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
