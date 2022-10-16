using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Mappings;
using ExchangeRateUpdater.Clients.Cnb.Mappings.ValueResolvers;
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
    protected virtual void ConfigureMapper(IMapperConfigurationExpression configure)
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
}