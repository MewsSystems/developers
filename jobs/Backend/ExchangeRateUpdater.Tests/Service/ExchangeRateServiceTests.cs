using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Service
{
    [TestFixture]
    internal class ExchangeRateServiceTests
    {
        private IExchangeRateProvider provider = null!;
        private ILogger<ExchangeRateService> logger = null!;

        [SetUp]
        public void SetUp()
        {
            provider = Substitute.For<IExchangeRateProvider>();
            logger = Substitute.For<ILogger<ExchangeRateService>>();
        }

        [Test]
        public async Task GetExchangeRatesFromStringList_ValidCodes_CallsProviderAndReturnsRates()
        {
            var now = DateTime.UtcNow;
            var expected = new[] { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m, now) };

            provider.GetExchangeRatesAsync(Arg.Is<IEnumerable<Currency>>(list => list.Select(c => c.Code).SequenceEqual(new[] { "USD" })))
                .Returns(Task.FromResult<IEnumerable<ExchangeRate>>(expected));

            var svc = new ExchangeRateService(provider, logger);
            var result = await svc.GetExchangeRatesFromStringList(new[] { "USD" });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToArray().Length, Is.EqualTo(1));
            Assert.That(result.First().SourceCurrency.Code, Is.EqualTo("USD"));
        }

        [Test]
        public async Task GetExchangeRatesFromStringList_InvalidCodes_SkipsInvalidAndLogsWarning()
        {
            var provider = Substitute.For<IExchangeRateProvider>();
            var logger = Substitute.For<ILogger<ExchangeRateService>>();

            var now = DateTime.UtcNow;
            var expected = new[] { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m, now) };

            provider.GetExchangeRatesAsync(Arg.Is<IEnumerable<Currency>>(list => list.Select(c => c.Code).SequenceEqual(new[] { "USD" })))
                .Returns(Task.FromResult<IEnumerable<ExchangeRate>>(expected));

            var svc = new ExchangeRateService(provider, logger);

            // "US" is invalid (length != 3) and should be skipped
            var result = await svc.GetExchangeRatesFromStringList(new[] { "USD", "US" });

            Assert.That(result, Is.Not.Null);
            Assert.That(result.First().SourceCurrency.Code, Is.EqualTo("USD"));

            // Verify a warning was logged for the invalid code
            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Test]
        public async Task GetExchangeRatesFromStringList_NoValidCodes_ReturnsEmptyAndLogsWarning()
        {
            var provider = Substitute.For<IExchangeRateProvider>();
            var logger = Substitute.For<ILogger<ExchangeRateService>>();

            var svc = new ExchangeRateService(provider, logger);

            var result = await svc.GetExchangeRatesFromStringList(new[] { "", " " });

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            // Should have logged a warning about no valid currency codes
            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}
