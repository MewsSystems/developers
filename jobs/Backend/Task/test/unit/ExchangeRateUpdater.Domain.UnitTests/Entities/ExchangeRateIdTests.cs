using System;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.UnitTests.Entities
{
    public class ExchangeRateIdTests
    {
        [Test]
        public void GetHashCode_ShouldReturnExpectedResult()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            exchangeRateId.GetHashCode().Should()
                .Be(HashCode.Combine(exchangeRateId.SourceCurrency, exchangeRateId.TargetCurrency));
        }
        
        [Test]
        public void Equals_ShouldReturnTrue_IfObjectsHaveSameReference()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            var exchangeRateId2 = exchangeRateId;
            exchangeRateId.Equals(exchangeRateId2).Should().BeTrue();
        }
        
        [Test]
        public void Equals_ShouldReturnTrue_IfObjectsHaveSameSourceAndTargetCurrencies()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            var exchangeRateId2 = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));;
            exchangeRateId.Equals(exchangeRateId2).Should().BeTrue();
        }
        
        
        [Test]
        public void Equals_ShouldReturnFalse_IfSecondObjectIsNull()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            exchangeRateId.Equals(null).Should().BeFalse();
        }
        
        
        [Test]
        public void Equals_ShouldReturnFalse_IfSecondObjectIsOfDifferentType()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            exchangeRateId.Equals(new object()).Should().BeFalse();
        }
        
        
        [Test]
        public void Equals_ShouldReturnFalse_IfObjectsDifferBySourceCurrency()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            var exchangeRateId2 = new ExchangeRateId(Currency.From("USD"), Currency.From("USD"));
            exchangeRateId.Equals(exchangeRateId2).Should().BeFalse();
        }
        
        
        [Test]
        public void Equals_ShouldReturnFalse_IfObjectsDifferByTargetCurrency()
        {
            var exchangeRateId = new ExchangeRateId(Currency.From("EUR"), Currency.From("EUR"));
            var exchangeRateId2 = new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
            exchangeRateId.Equals(exchangeRateId2).Should().BeFalse();
        }
    }
}   