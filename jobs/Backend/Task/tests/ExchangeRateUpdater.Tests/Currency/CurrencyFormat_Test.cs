using System;
using System.DirectoryServices.ActiveDirectory;
using ExchangeRateUpdater.Exceptions;
using Xunit;

namespace ExchangeRateUpdater.Tests.Currency
{
    public class CurrencyFormat_Test : TestBase
    {
        [Fact]
        public void Should_Throw_Exception_When_Invalid_Currency_Format()
        {
            Assert.Throws<NotValidCurrencyCodeException>(() => { _ = new Domain.Currency("XYZUS"); });
            Assert.Throws<NotValidCurrencyCodeException>(() => { _ = new Domain.Currency("dd"); });
        }
    }
}