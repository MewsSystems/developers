using System;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Parsers
{
    public class ParserException : Exception
    {
        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}