using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdater.Tests.Shared.Mapping;

public abstract class MappingProfileTestBase
{
    private readonly MapperConfiguration _mapperConfiguration;
    protected readonly Mock<IConfiguration> ConfigurationMock;

    protected MappingProfileTestBase()
    {
        _mapperConfiguration = new MapperConfiguration(cfg => ConfigureMapper(cfg));

        Mapper = _mapperConfiguration.CreateMapper();
        
        ConfigurationMock = new Mock<IConfiguration>();
        ConfigurationMock.Setup(configuration => configuration["ExchangeRateCurrency"]).Returns("CZK");
    }

    protected IMapper Mapper { get; }

    /// <summary>
    /// Gets the subject profile under test.
    /// </summary>
    /// <returns></returns>
    protected abstract void ConfigureMapper(IMapperConfigurationExpression configure);

    [Fact]
    public void Mapper_configuration_should_be_valid()
    {
        // Act
        Action act = () => _mapperConfiguration.AssertConfigurationIsValid();

        // Assert
        act.Should().NotThrow();
    }
}