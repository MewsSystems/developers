using Mews.ExchangeRateProvider.Api.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Mews.ExchangeRateProvider.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateProvider.Api.Test.Controllers
{
    public class RatesControllerTests
    {
        [Fact]
        public async Task GetExchangeRatesDaily_OnValidRequest_ShouldReturnOkStatusCode()
        {
            // Arrange
            var rateRepositoryMock = new Mock<IRateRepository>();
            var loggerMock = new Mock<ILogger<RatesController>>();

            var controller = new RatesController(rateRepositoryMock.Object, loggerMock.Object);

            var date = "2023-11-13";
            var lang = "en";
            var getAllRates = true;

            // Act
            var result = await controller.GetExchangeRatesDaily(date, lang, getAllRates);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            rateRepositoryMock.Verify(repo => repo.GetDailyRatesAsync(date, lang, getAllRates), Times.Never);
        }
    }
}


