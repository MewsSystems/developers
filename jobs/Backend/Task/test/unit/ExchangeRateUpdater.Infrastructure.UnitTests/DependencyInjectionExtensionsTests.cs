using System;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateUpdater.Infrastructure.UnitTests
{

    public class DependencyInjectionExtensionsTests
    {
        [Test]
        public void AddInfrastructureLayer_ShouldNotThrowException()
        {
            var services = Substitute.For<IServiceCollection>();
            var configuration = Substitute.For<IConfiguration>();
            var configurationSection = Substitute.For<IConfigurationSection>();
            configurationSection["BaseUrl"].Returns("http://host");
            configuration.GetSection(nameof(IExchangeRateApiServiceClient)).Returns(configurationSection);
            Action act = () => services.AddInfrastructureLayer(configuration);
            act.Should().NotThrow();
        }
    }
}