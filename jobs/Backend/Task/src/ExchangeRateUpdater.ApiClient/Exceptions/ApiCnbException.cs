﻿namespace ExchangeRateUpdater.ApiClient.Exceptions
{
    public class ApiCnbException : Exception
    {
        public ApiCnbException(string message, Exception inner) : base(message,inner) { }
    }
}
