using System;

namespace ExchangeRateUpdater.Domain.DomainExceptions
{
    public class InvalidCurrencyFormatException : Exception
    {
        public InvalidCurrencyFormatException(string currencyCode) : base($"Currency {currencyCode} is not following ISO 4217. e.g: CZN") { }
    }
}
