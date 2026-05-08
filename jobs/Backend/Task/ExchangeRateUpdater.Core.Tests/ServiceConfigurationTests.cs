using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Configuration.Options;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Core.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ExchangeRateUpdater.Core.Tests;

public class ServiceConfigurationTests
{
    [Fact]
    public void AddCore_BindsCurrencies_AndRegistersProvider()
    {
        // Arrange
        var inMemory = new[]
        {
            new KeyValuePair<string, string?>("Currencies:0", "usd"),
            new KeyValuePair<string, string?>("Currencies:1", " EUR ")
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemory).Build();
        var services = new ServiceCollection();

        services.AddSingleton(Moq.Mock.Of<ApiVendors.IExchangeRateVendor>());

        // Act
        services.AddCore(configuration);
        var sp = services.BuildServiceProvider();

        // Assert
        sp.GetService<IExchangeRateProvider>().Should().NotBeNull();

        var opts = sp.GetRequiredService<IOptions<CurrencyOptions>>().Value;
        opts.Currencies.Should().HaveCount(2);
        opts.Currencies.Select(c => c.ToString()).Should().BeEquivalentTo(new[] { "usd", " EUR " }.Select(s => new Currency(s).ToString()));
    }
}
