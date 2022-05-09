using Core.Infra.Dtos;
using Core.Infra.Mappers;
using FluentAssertions;
using System.IO;
using Xunit;

namespace Tests.Infra.Mappers
{
    public class ExchangeRateDtoMapperTests
    {
        private IExchangeRateDtoMapper _exchangeRateDtoMapper;

        public ExchangeRateDtoMapperTests()
        {
            _exchangeRateDtoMapper = new ExchangeRateDtoMapper();
        }

        [Fact(DisplayName = "ERDM-001: Map ExchangeRateDto collection to ExchangeRate collection.")]
        public void ERDM001()
        {
            // arrange
            var source = File.ReadAllText("Content/exchangeRateContent_Successful.txt");
            var expectedExchangeRateDtos = new ExchangeRateDto[]
            {
                new ExchangeRateDto("Turkey", "lira", "1", "TRY", "1.573"),
                new ExchangeRateDto("Thailand", "baht", "100", "THB", "68.273"),
                new ExchangeRateDto("EMU", "euro", "1", "EUR", "24.605"),
                new ExchangeRateDto("United Kingdom", "pound", "1", "GBP", "29.322"),
                new ExchangeRateDto("USA", "dollar", "1", "USD", "23.344")
            };

            // act
            var rateDtos = _exchangeRateDtoMapper.Map(source);

            // assert
            rateDtos.Should().NotBeNull();
            rateDtos.IsSuccess.Should().BeTrue();
            rateDtos.Value.Should().BeEquivalentTo(expectedExchangeRateDtos);
        }
    }
}
