using AutoMapper;

namespace ExchangeRateService.UnitTests;

public class AutoMapperTests
{
    [Fact]
    public void AssertConfigurationIsValid_ForAllMaps_IsOk()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.AddMaps(typeof(Program).Assembly);
        });

        config.AssertConfigurationIsValid();
    }
}