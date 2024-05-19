using AutoFixture;
using ExchangeRateUpdater.API.Controllers;
using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.API.Models.ResponseModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ExchangeRateUpdater.API.UnitTests.Controllers.ExchangeRates
{
    public class WhenGettingExchangeRates
    {
        private ExchangeRatesController _controller;
        private Mock<IMediator> _mediator;
        private Mock<IValidator<GetExchangeRatesRequest>> _validator;
        private Mock<ILogger<ExchangeRatesController>> _logger; 
        private GetExchangeRatesQueryResult _expectedQueryResult;
        private readonly Fixture _fixture = new();
        private readonly string _queriedExRateGBP = "GBP";
        private readonly ExchangeRate _expectedExchangeRateGBP = new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 23.3m);
        private readonly string _expectedExchangeRateText = "GBP/CZK=23.3";

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _validator = new Mock<IValidator<GetExchangeRatesRequest>>();
            _logger = new Mock<ILogger<ExchangeRatesController>>();
            _controller = new ExchangeRatesController(_mediator.Object, _logger.Object, _validator.Object);
            _expectedQueryResult = _fixture.Build<GetExchangeRatesQueryResult>()
                .With(r => r.ExchangeRates, new List<ExchangeRate>() { _expectedExchangeRateGBP })
                .Create();
            _mediator.Setup(x => x.Send(It.Is<GetExchangeRatesQuery>(q => q.Currencies.First().Equals(_queriedExRateGBP)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedQueryResult);
        }

        [Test]
        public async Task Then_The_Exchange_Rate_Is_Returned() 
        {
            // Arrange
            var requestedCurrencies = new List<string>() { "GBP" };
            var request = new GetExchangeRatesRequest() { Currencies = requestedCurrencies };
            _validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult { });

            // Act
            var actual = await _controller.GetExchangeRates(request);

            // Assert
            var result = actual as OkObjectResult;
            Assert.IsNotNull(result?.StatusCode);
            Assert.That(HttpStatusCode.OK.Equals((HttpStatusCode)result.StatusCode));
            var resultObject = result.Value as GetExchangeRatesResponse;
            Assert.That(resultObject.ExchangeRates.First().Equals(_expectedExchangeRateText));
        }

        [Test]
        public async Task Then_If_Validation_Fails_BadRequest_Returned()
        {
            // Arrange
            var requestedCurrencies = new List<string>() { "GIBBERISH" };
            var request = new GetExchangeRatesRequest() { Currencies = requestedCurrencies };
            _validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure> { new ValidationFailure("errorProperty", "error") } });

            // Act
            var actual = await _controller.GetExchangeRates(request);

            // Assert
            var result = actual as BadRequestObjectResult;
            Assert.IsNotNull(result?.StatusCode);
            Assert.That(HttpStatusCode.BadRequest.Equals((HttpStatusCode)result.StatusCode));
        }
    }
}
