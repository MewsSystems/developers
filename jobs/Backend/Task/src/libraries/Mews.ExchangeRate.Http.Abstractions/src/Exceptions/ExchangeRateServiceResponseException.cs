using System;
using System.Collections.Generic;
using System.Text;

namespace Mews.ExchangeRate.Http.Abstractions.Exceptions
{
    /// <summary>
    /// Custom exception for handling response from the ExchangeRateService.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ExchangeRateServiceResponseException : Exception
    {
        public ExchangeRateServiceResponseException() : base() { }

        public ExchangeRateServiceResponseException(string message) : base(message) { }

        public ExchangeRateServiceResponseException(string message, Exception innerException) : base(message, innerException) { }

        public ExchangeRateServiceResponseException(string messageFormat, params object[] args) : this(string.Format(messageFormat, args)) { }
        public ExchangeRateServiceResponseException(Exception innerException, string messageFormat, params object[] args) : this(string.Format(messageFormat, args), innerException) { }
    }
}
