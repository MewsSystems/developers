using ExchangeRateUpdater.Domain.Ports;
using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;

namespace Adapter.ExchangeRateProvider.CzechNationalBank.Tests.Integration
{
    [TestFixture]
    internal class GetDefaultUnitRates
    {
        private TestHttpClientFactory _httpClientFactory;
        private ILogger _logger;

        

        [Test]
        public async Task WhenCallingCzechNationalToGetDefaultUnitRates_ShouldReturnAListOfUnitExchangeRates()
        {
            // act
            var sut = CreateSut();

            // assert
            var result = await sut.GetDefaultUnitRates();
            result.ToList().Count.Should().BeGreaterThan(0);
        }

        private IExchangeRateProviderRepository CreateSut()
        {
            return new CzechNationalBankRepository(_httpClientFactory, _logger);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
            _httpClientFactory = new TestHttpClientFactory("https://www.cnb.cz/en/");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _httpClientFactory.Dispose();
        }
    }
}
