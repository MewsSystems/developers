using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException() : base() { }
        public InvalidCurrencyException(string? message) : base(message) { }
    }
}
