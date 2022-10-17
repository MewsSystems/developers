using AutoMapper;
using ExchangeRateUpdater.Domain.Mappings;
using ExchangeRateUpdater.Domain.Mappings.ValueResolvers;
using ExchangeRateUpdater.Domain.Options;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests.Shared.Mapping;

public abstract class MappingProfileTestBase
{
    protected Mock<IOptions<ApplicationOptions>> ApplicationOptions;
    
    protected MappingProfileTestBase()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => ConfigureMapper(cfg));

        Mapper = mapperConfiguration.CreateMapper();
        
        var applicationOptions = new ApplicationOptions
        {
            ExchangeRateCurrency = "CZK",
            EnableInMemoryCache = true
        };

        ApplicationOptions = new Mock<IOptions<ApplicationOptions>>();
        ApplicationOptions.Setup(ap => ap.Value).Returns(applicationOptions);
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
            return new ExchangeRateValueResolver(ApplicationOptions.Object);
        }

        return Activator.CreateInstance(type);
    }
}