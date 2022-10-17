using AutoMapper;
using ExchangeRateUpdater.Domain.Mappings;
using ExchangeRateUpdater.Domain.Mappings.ValueResolvers;
using ExchangeRateUpdater.Domain.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests.Shared;

public abstract class TestBase
{
    protected Mock<IOptions<ApplicationOptions>> ApplicationOptions { get; set; }
    protected IMapper Mapper { get; }

    protected TestBase()
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