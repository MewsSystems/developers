using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.UnitTests.Entities
{
    public class ExchangeRateTests: EntityTestBase<ExchangeRate, ExchangeRateId>
    {
        protected override ExchangeRate CreateEntity(ExchangeRateId id)
        {
            return new ExchangeRate(id, 1);
        }

        protected override ExchangeRateId CreateId()
        {
            return new ExchangeRateId(Currency.From("EUR"), Currency.From("USD"));
        }

        [Test]
        public void ToString_ShouldReturnExpectedResult()
        {
            var id = CreateId();
            var entity = CreateEntity(id);
            const string expectedResult = "EUR/USD=1";
            entity.ToString().Should().Be(expectedResult);
        }
    }
}