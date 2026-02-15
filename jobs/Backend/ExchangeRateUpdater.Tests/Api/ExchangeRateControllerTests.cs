using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using ExchangeRateUpdater.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Api
{
    [TestFixture]
    internal class ExchangeRateControllerTests
    {
        private IExchangeRateService _service = null!;
        private ILogger<ExchangeRateController> _logger = null!;
        private ExchangeRateController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = Substitute.For<IExchangeRateService>();
            _logger = Substitute.For<ILogger<ExchangeRateController>>();
            _controller = new ExchangeRateController(_service, _logger);
        }

        [Test]
        public async Task GetExchangeRates_ReturnsOkWithRates_WhenServiceReturnsRates()
        {
            var now = DateTime.UtcNow;
            var rates = new[] { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m, now) };
            _service.GetExchangeRatesFromStringList(Arg.Any<IEnumerable<string>>()).Returns(Task.FromResult<IEnumerable<ExchangeRate>>(rates));

            var result = await _controller.GetExchangeRates("USD, EUR");

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var ok = (OkObjectResult)result.Result!;
            Assert.That(ok.Value, Is.InstanceOf<IEnumerable<ExchangeRate>>());
            var returned = ((IEnumerable<ExchangeRate>)ok.Value!).ToArray();
            Assert.That(returned.Length, Is.EqualTo(1));
            Assert.That(returned[0].SourceCurrency.Code, Is.EqualTo("USD"));

            // Ensure service was called with the split and trimmed values
            await _service.Received(1).GetExchangeRatesFromStringList(Arg.Is<IEnumerable<string>>(s => s.SequenceEqual(new[] { "USD", "EUR" })));
        }

        [Test]
        public async Task GetExchangeRates_ReturnsBadRequest_WhenNoRates()
        {
            _service.GetExchangeRatesFromStringList(Arg.Any<IEnumerable<string>>()).Returns(Task.FromResult<IEnumerable<ExchangeRate>>(Array.Empty<ExchangeRate>()));

            var result = await _controller.GetExchangeRates("USD");

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var bad = (BadRequestObjectResult)result.Result!;
            Assert.That(bad.Value, Is.EqualTo("No exchange rates found for the provided currency codes."));

            // Logger should have been called to warn
            _logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}
