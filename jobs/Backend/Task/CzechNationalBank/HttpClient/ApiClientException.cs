using System;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient
{
    public class ApiClientException : Exception
    {
        public ApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}