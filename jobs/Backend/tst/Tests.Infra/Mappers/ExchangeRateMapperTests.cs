using Core.Domain.Models;
using Core.Infra.Dtos;
using Core.Infra.Mappers;
using FluentAssertions;
using Xunit;

namespace Tests.Infra.Mappers
{
    public class ExchangeRateMapperTests
    {
        private IExchangeRateMapper _exchangeRateMapper;

        public ExchangeRateMapperTests()
        {
            _exchangeRateMapper = new ExchangeRateMapper();
        }

        [Fact(DisplayName = "ERM-001: Map ExchangeRateDto collection to ExchangeRate collection.")]
        public void ERM001()
        {
            // Arrange

            var sourceExchangeRateDtos = new ExchangeRateDto[]
            {
                new ExchangeRateDto("Turkey", "lira", "1", "TRY", "1.573"),
                new ExchangeRateDto("Thailand", "baht", "100", "THB", "68.273"),
                new ExchangeRateDto("EMU", "euro", "1", "EUR", "24.605"),
                new ExchangeRateDto("United Kingdom", "pound", "1", "GBP", "29.322"),
                new ExchangeRateDto("USA", "dollar", "1", "USD", "23.344")
            };

            var expectedExchangeRates = new ExchangeRate[]
            {
                new ExchangeRate(new Currency("TRY"), new Currency("CZK"), 1.573m),
                new ExchangeRate(new Currency("THB"), new Currency("CZK"), 68.273m/100),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.605m),
                new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 29.322m),
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.344m)
            };

            // Act
            var mappedEntity = _exchangeRateMapper.Map(sourceExchangeRateDtos);

            // Assert
            mappedEntity.Should().NotBeNull();
            mappedEntity.IsSuccess.Should().BeTrue();
            mappedEntity.Value.Should().BeEquivalentTo(expectedExchangeRates);
        }
    }
}
