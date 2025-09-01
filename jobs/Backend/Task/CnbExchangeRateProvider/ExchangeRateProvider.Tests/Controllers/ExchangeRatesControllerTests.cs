using ExchangeRateProvider.Api.Controllers;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateProvider.Tests.Controllers
{
    public class ExchangeRatesControllerTests
    {
        [Fact]
        public async Task Returns_Rates_With_Target_Ignored_And_No_Date()
        {
            var mediator = new Mock<IMediator>();
            var usd = new Currency("USD");
            var eur = new Currency("EUR");

            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>
                {
                    new ExchangeRate(usd, eur, 0.9m)
                });

            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, new ConfigurationBuilder().Build());
            var result = await controller.GetExchangeRates("USD");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var rates = Assert.IsAssignableFrom<IEnumerable<ExchangeRate>>(ok.Value);
            Assert.Single(rates);
            Assert.Equal(0.9m, rates.First().Value);
        }

        [Fact]
        public async Task Returns_Empty_List_When_No_Rates_Found()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>());

            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, new ConfigurationBuilder().Build());
            var result = await controller.GetExchangeRates("USD");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var rates = Assert.IsAssignableFrom<IEnumerable<ExchangeRate>>(ok.Value);
            Assert.Empty(rates);
        }

        [Fact]
        public async Task Handles_Single_Currency_Code()
        {
            var mediator = new Mock<IMediator>();
            var usd = new Currency("USD");
            var czk = new Currency("CZK");

            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate> { new ExchangeRate(usd, czk, 22.5m) });

            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, new ConfigurationBuilder().Build());
            var result = await controller.GetExchangeRates("USD");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var rates = Assert.IsAssignableFrom<IEnumerable<ExchangeRate>>(ok.Value);
            Assert.Single(rates);
            Assert.Equal("USD", rates.First().SourceCurrency.Code);
            Assert.Equal("CZK", rates.First().TargetCurrency.Code);
        }

        [Fact]
        public async Task Handles_Multiple_Currency_Codes_With_Spaces()
        {
            var mediator = new Mock<IMediator>();
            var usd = new Currency("USD");
            var eur = new Currency("EUR");
            var czk = new Currency("CZK");

            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>
                {
                    new ExchangeRate(usd, czk, 22.5m),
                    new ExchangeRate(eur, czk, 24.0m)
                });

            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, new ConfigurationBuilder().Build());
            var result = await controller.GetExchangeRates("USD, EUR, GBP");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var rates = Assert.IsAssignableFrom<IEnumerable<ExchangeRate>>(ok.Value);
            Assert.Equal(2, rates.Count()); // GBP not returned by mock
        }

        [Fact]
        public async Task Configuration_Is_Used_For_MaxCurrencies()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:MaxCurrencies"] = "5"
                })
                .Build();

            var mediator = new Mock<IMediator>();
            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, configuration);

            // This should work with 5 currencies
            var validCurrencies = new[] { "USD", "EUR", "GBP", "JPY", "CAD" };
            var currencies = string.Join(",", validCurrencies);
            var result = await controller.GetExchangeRates(currencies);

            // Should not be BadRequest since we're under the limit
            Assert.IsNotType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Handles_Currency_Codes_With_Special_Characters()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>());

            var controller = new ExchangeRatesController(mediator.Object, NullLogger<ExchangeRatesController>.Instance, new ConfigurationBuilder().Build());

            // Test with various currency code formats
            var result = await controller.GetExchangeRates("USD123,EUR");

            // Should process without throwing
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Logs_Information_On_Successful_Request()
        {
            var logger = new Mock<ILogger<ExchangeRatesController>>();
            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate> { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22m) });

            var controller = new ExchangeRatesController(mediator.Object, logger.Object, new ConfigurationBuilder().Build());
            await controller.GetExchangeRates("USD");

            logger.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Fetching exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Logs_Information_On_Empty_CurrencyCodes()
        {
            var logger = new Mock<ILogger<ExchangeRatesController>>();
            var mediator = new Mock<IMediator>();

            var controller = new ExchangeRatesController(mediator.Object, logger.Object, new ConfigurationBuilder().Build());
            await controller.GetExchangeRates("");

            logger.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("No currency codes provided")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Logs_Error_On_Exception()
        {
            var logger = new Mock<ILogger<ExchangeRatesController>>();
            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test error"));

            var controller = new ExchangeRatesController(mediator.Object, logger.Object, new ConfigurationBuilder().Build());
            await controller.GetExchangeRates("USD");

            logger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Unexpected error")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}


