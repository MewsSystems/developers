using System;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient.Builders
{
    public class HttpRequestBuilderException : Exception
    {
        public HttpRequestBuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}