using System;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient
{
    public class ApiClientException : Exception
    {
        public ApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}