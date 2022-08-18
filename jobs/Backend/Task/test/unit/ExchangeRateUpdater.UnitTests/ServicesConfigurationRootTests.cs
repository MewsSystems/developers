using System;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.UnitTests
{
    public class ServicesConfigurationRootTests
    {
        [Test]
        public void BuildServiceProvider_ShouldNotThrowException()
        {
            Action act = () => ServicesConfigurationRoot.BuildServiceProvider();
            act.Should().NotThrow();
        }
    }
}