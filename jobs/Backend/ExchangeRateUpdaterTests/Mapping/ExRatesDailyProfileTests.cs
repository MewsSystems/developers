using AutoMapper;
using AutoFixture;
using Xunit;
using FluentAssertions;
using CNB = Cnb.Api.Client;

namespace ExchangeRateUpdater.Mapping.Tests
{

    public class ExRatesDailyProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public ExRatesDailyProfileTests()
        {
            _fixture = new Fixture();

            var config = new MapperConfiguration((cfg) =>
            {
                cfg.AddProfile<ExRatesDailyProfile>();
            });

            _mapper = config.CreateMapper();
        }


        [Fact]
        public void ExRatesDailyProfile_ConfigurationIsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void ExRateDailyRest_To_ExchangeRate_Mapping_Should_Be_Valid()
        {
            // Arrange
            var source = _fixture.Build<CNB.ExRateDailyRest>()
                .With(x => x.CurrencyCode, "USD")
                .With(x => x.Rate, 23.048)
                .With(x => x.Amount, 1)
                .Create();

            // Act
            var result = _mapper.Map<ExchangeRate>(source);

            // Assert
            result.Should().NotBeNull();
            result.SourceCurrency.Code.Should().Be("USD");
            result.TargetCurrency.Code.Should().Be("CZK");
            result.Value.Should().Be((decimal)(source.Rate ?? 0) / (source.Amount ?? 1));
        }
    }
}