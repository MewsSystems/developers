using System;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient.Parsers
{
    public class ParserException : Exception
    {
        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}