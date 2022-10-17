using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Tests.Shared.Builders;
using ExchangeRateUpdater.Tests.Shared.Mapping;
using FluentAssertions;

namespace ExchangeRateUpdater.Domain.Tests.Mappings
{
    public class ExchangeRateMappingProfileTests : MappingProfileTestBase
    {
        [Theory]
        [MemberData(nameof(MappingTestCases))]
        public void Given_source_when_mapping_source_to_destination_then_it_should_map_as_expected(object src, object expectedDest)
        {
            // Arrange
            var srcType = src.GetType();
            var destType = expectedDest.GetType();

            // Act
            var dest = Mapper.Map(src, srcType, destType);

            // Assert
            dest.Should().BeEquivalentTo(expectedDest);
            expectedDest.GetType().Should().Be(destType);
        }

        public static IEnumerable<object[]> MappingTestCases()
        {
            yield return new object[]
            {
                new ExchangeRatesResponse
                {
                    ExchangeRates = { new ExchangeRateDtoBuilder().WithAmount(1).WithCode("TRY").WithCountry("Turkey").WithCurrency("lira").WithRate(5).Build() }
                },
                new List<ExchangeRate>
                {
                    new ExchangeRateBuilder().WithSourceCurrency("TRY").WithTargetCurrency("CZK").WithValue(5).Build()
                }
            };
        }
    }
}