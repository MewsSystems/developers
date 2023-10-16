using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.DtoMappers;
using Mews.ExchangeRateUpdater.Services.Models;

namespace Mews.ExchangeRateUpdater.Tests.Services.DtoMappers
{
    [TestFixture]
    public class CurrencyDtoMapperTests
    {
        [Test]
        public void ToCurrencyDto_WhenCalledWithNullCurrencyModel_ReturnsNull()
        {
            // Arrange
            var currencyModel = new CurrencyModel { Code = "GBP" };

            // Act
            var actual = CurrencyDtoMapper.ToCurrencyDto(currencyModel);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<CurrencyDto>());
            Assert.That(actual.Code, Is.EqualTo(currencyModel.Code));
        }

        [Test]
        public void ToCurrencyDto_WhenCalledWithCurrencyModel_ReturnsCurrencyDto()
        {
            // Arrange
            var currencyModel = new CurrencyModel { Code = "GBP" };

            // Act
            var actual = CurrencyDtoMapper.ToCurrencyDto(currencyModel);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<CurrencyDto>());
            Assert.That(actual.Code, Is.EqualTo(currencyModel.Code));
        }
    }
}
