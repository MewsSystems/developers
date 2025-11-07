using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Infrastructure.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.Tests;

public class ServiceConfigurationTests
{
    [Fact]
    public void AddInfrastructure_Throws_WhenBaseUrlMissing()
    {
        var settings = new[]
        {
            new KeyValuePair<string, string?>("CurrencyApi:ApiKey", "MY_KEY")
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var services = new ServiceCollection();

        Action act = () => services.AddInfrastructure(configuration);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CurrencyApi:BaseUrl*");
    }

    [Fact]
    public void AddInfrastructure_Throws_WhenApiKeyMissing()
    {
        var settings = new[]
        {
            new KeyValuePair<string, string?>("CurrencyApi:BaseUrl", "https://api.currencyapi.com/v3/")
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var services = new ServiceCollection();

        Action act = () => services.AddInfrastructure(configuration);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CurrencyApi:ApiKey*");
    }

    [Fact]
    public void AddInfrastructure_Resolves_TypedVendor()
    {
        var settings = new[]
        {
            new KeyValuePair<string, string?>("CurrencyApi:BaseUrl", "https://api.currencyapi.com/v3/"),
            new KeyValuePair<string, string?>("CurrencyApi:ApiKey", "MY_KEY")
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var services = new ServiceCollection();

        services.AddInfrastructure(configuration);
        var sp = services.BuildServiceProvider();

        sp.GetService<IExchangeRateVendor>().Should().NotBeNull();
    }
}
