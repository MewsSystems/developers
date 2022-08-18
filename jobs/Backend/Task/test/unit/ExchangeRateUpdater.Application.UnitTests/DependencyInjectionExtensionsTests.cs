using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateUpdater.Application.UnitTests
{
    public class DependencyInjectionExtensionsTests
    {
        [Test]
        public void AddApplicationLayer_ShouldNotThrowException()
        {
            var services = Substitute.For<IServiceCollection>();
            Action act = () => services.AddApplicationLayer();
            act.Should().NotThrow();
        }
    }
}