using System;

namespace ExchangeRateUpdater.ExchangeRate.Exception
{
    /// <summary>
    /// Represents errors that occur during exchange rate updating operations.
    /// </summary>
    public class ExchangeRateUpdaterException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateUpdaterException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExchangeRateUpdaterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateUpdaterException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ExchangeRateUpdaterException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
