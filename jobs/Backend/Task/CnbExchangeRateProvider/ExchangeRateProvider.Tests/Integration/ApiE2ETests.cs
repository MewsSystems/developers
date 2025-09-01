using ExchangeRateProvider.Api.Controllers;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateProvider.Tests.Integration
{
    /// <summary>
    /// E2E test for critical user flow: API request to response.
    /// </summary>
    public class ApiE2ETests
    {
        [Fact]
        public async Task GetExchangeRates_ReturnsValidRates_ForValidRequest()
        {
            var mockMediator = new Mock<IMediator>();

            var expectedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.1m)
            };

            mockMediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRates);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:MaxCurrencies"] = "20"
                })
                .Build();
            var controller = new ExchangeRatesController(mockMediator.Object, NullLogger<ExchangeRatesController>.Instance, configuration);

            // Act - Simulate API call
            var result = await controller.GetExchangeRates("USD,EUR");

            // Assert - Business rules
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRates = Assert.IsType<List<ExchangeRate>>(okResult.Value);

            Assert.Equal(2, returnedRates.Count);
            Assert.All(returnedRates, rate => Assert.Equal("CZK", rate.TargetCurrency.Code));
            Assert.Contains(returnedRates, r => r.SourceCurrency.Code == "USD");
            Assert.Contains(returnedRates, r => r.SourceCurrency.Code == "EUR");
        }

        [Fact]
        public async Task GetExchangeRates_HandlesEmptyRequest_Gracefully()
        {
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExchangeRate>());

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:MaxCurrencies"] = "20"
                })
                .Build();
            var controller = new ExchangeRatesController(mockMediator.Object, NullLogger<ExchangeRatesController>.Instance, configuration);

            // Act
            var result = await controller.GetExchangeRates("");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRates = Assert.IsType<List<ExchangeRate>>(okResult.Value);
            Assert.Empty(returnedRates);
        }

        [Fact]
        public async Task GetExchangeRates_HandlesMediatorException_WithProperResponse()
        {
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(It.IsAny<GetExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Service unavailable"));

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ExchangeRateProvider:MaxCurrencies"] = "20"
                })
                .Build();
            var controller = new ExchangeRatesController(mockMediator.Object, NullLogger<ExchangeRatesController>.Instance, configuration);

            // Act
            var result = await controller.GetExchangeRates("USD");

            // Assert - Should return 500 status code
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}