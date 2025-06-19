using ExchangeRateProviderAPI_PaolaRojas.Controllers;
using ExchangeRateProviderAPI_PaolaRojas.Models;
using ExchangeRateProviderAPI_PaolaRojas.Models.Requests;
using ExchangeRateProviderAPI_PaolaRojas.Models.Responses;
using ExchangeRateProviderAPI_PaolaRojas.UnitTests.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProviderAPI_PaolaRojas.UnitTests.Controllers
{
    public class ExchangeRatesControllerTests
    {
        [Fact]
        public async Task Should_Return_200_With_ExchangeRates()
        {
            var testRequest = new CurrencyRequest
            {
                Currencies = new List<Currency> { new("USD") }
            };

            var response = new ExchangeRateResponse
            {
                ExchangeRates = new[]
                {
                    new ExchangeRate(new Currency("USD"), new Currency("CZK"), 21.90m)
                }
            };

            var mockService = MockExchangeRateService.WithResult(response);
            var controller = new ExchangeRatesController(mockService.Object);

            var result = await controller.GetExchangeRates(testRequest);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task Should_Return_404_When_ExchangeRates_Are_Empty()
        {
            var testRequest = new CurrencyRequest
            {
                Currencies = new List<Currency> { new("XYZ") }
            };

            var emptyResponse = new ExchangeRateResponse { ExchangeRates = [] };
            var mockService = MockExchangeRateService.WithResult(emptyResponse);
            var controller = new ExchangeRatesController(mockService.Object);

            var result = await controller.GetExchangeRates(testRequest);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Should_Return_404_When_Response_Is_Null()
        {
            var testRequest = new CurrencyRequest
            {
                Currencies = [new("USD")]
            };

            var mockService = MockExchangeRateService.WithResult(null);
            var controller = new ExchangeRatesController(mockService.Object);

            var result = await controller.GetExchangeRates(testRequest);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Should_Return_404_For_Empty_Input_List()
        {
            var testRequest = new CurrencyRequest { Currencies = [] };

            var mockService = MockExchangeRateService.WithResult(new ExchangeRateResponse { ExchangeRates = [] });
            var controller = new ExchangeRatesController(mockService.Object);

            var result = await controller.GetExchangeRates(testRequest);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}