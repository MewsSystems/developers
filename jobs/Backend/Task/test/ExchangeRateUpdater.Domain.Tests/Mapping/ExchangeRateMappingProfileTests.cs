using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Mappings;
using ExchangeRateUpdater.Domain.Mappings.ValueResolvers;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Tests.Shared.Builders;
using ExchangeRateUpdater.Tests.Shared.Mapping;
using FluentAssertions;

namespace ExchangeRateUpdater.Domain.Tests.Mapping
{
    public class ExchangeRateMappingProfileTests : MappingProfileTestBase
    {
        protected override void ConfigureMapper(IMapperConfigurationExpression configure)
        {
            configure.AddProfile<ExchangeRateMappingProfile>();
            configure.ConstructServicesUsing(GetCustomServicesResolver);
        }

        private object GetCustomServicesResolver(Type type)
        {
            if (type == typeof(ExchangeRateValueResolver))
            {
                return new ExchangeRateValueResolver(ConfigurationMock.Object);
            }

            return Activator.CreateInstance(type);
        }

        [Theory]
        [MemberData(nameof(MappingTestCases))]
        public void When_mapping_source_to_destination_it_should_map_as_expected(object src, object expectedDest)
        {
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