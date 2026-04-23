using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Presentation.Contracts.Requests;
using ExchangeRateUpdater.Api.Presentation.Contracts.Responses;
using ExchangeRateUpdater.Api.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Api.Tests.Presentation.Controllers;

public class ExchangeRatesControllerTests
{
    private readonly IFixture _fixture;
    private readonly IExchangeRateProvider _provider;
    private readonly ExchangeRatesController _sut;

    public ExchangeRatesControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _provider = _fixture.Freeze<IExchangeRateProvider>();
        
        _sut = new ExchangeRatesController(_provider)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    private static Result<IReadOnlyCollection<ExchangeRate>> SuccessRates(
        params ExchangeRate[] rates) =>
        Result<IReadOnlyCollection<ExchangeRate>>.Success(rates);

    private static ExchangeRate CreateRate(string source, string target, decimal rate) =>
        ExchangeRate.Create(source, target, rate, 1, new DateOnly(2025, 6, 15)).Value;

    public class GetExchangeRates : ExchangeRatesControllerTests
    {
        [Fact]
        public async Task GetExchangeRates_WithMultipleCurrencies_ReturnsAllRatesWrappedInApiResponse()
        {
            // Arrange
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m),
                CreateRate("EUR", "CZK", 25.10m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRatesRequest();

            // Act
            var result = await _sut.GetExchangeRates(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ExchangeRateResponse>>>(okResult.Value);
            Assert.NotNull(apiResponse.Data);
            var list = apiResponse.Data.ToArray();
            Assert.Equal(2, list.Length);
            Assert.Contains(list, r => r.SourceCurrency == "USD" && r.Rate == 23.45m);
            Assert.Contains(list, r => r.SourceCurrency == "EUR" && r.Rate == 25.10m);
        }

        [Fact]
        public async Task GetExchangeRates_WithCurrencyFilter_ReturnsOnlyMatchingCurrencies()
        {
            // Arrange
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m),
                CreateRate("EUR", "CZK", 25.10m),
                CreateRate("GBP", "CZK", 29.80m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRatesRequest { Currencies = "USD,GBP" };

            // Act
            var result = await _sut.GetExchangeRates(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ExchangeRateResponse>>>(okResult.Value);
            Assert.NotNull(apiResponse.Data);
            var list = apiResponse.Data.ToArray();
            Assert.Equal(2, list.Length);
            Assert.Contains(list, r => r.SourceCurrency == "USD");
            Assert.Contains(list, r => r.SourceCurrency == "GBP");
            Assert.DoesNotContain(list, r => r.SourceCurrency == "EUR");
        }

        [Fact]
        public async Task GetExchangeRates_WithSpecificDate_PassesDateToProvider()
        {
            // Arrange
            var rates = SuccessRates();
            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRatesRequest { Date = "2025-06-15" };

            // Act
            await _sut.GetExchangeRates(request, CancellationToken.None);

            // Assert
            await _provider.Received(1).GetDailyRatesAsync(
                new DateOnly(2025, 6, 15), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetExchangeRates_WhenProviderUnavailable_Returns502()
        {
            // Arrange
            var failure = Result<IReadOnlyCollection<ExchangeRate>>.Failure(
                ExchangeRateErrors.SourceUnavailable);

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(failure);

            var request = new GetExchangeRatesRequest();

            // Act
            var result = await _sut.GetExchangeRates(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status502BadGateway, objectResult.StatusCode);
        }
    }

    public class GetExchangeRate : ExchangeRatesControllerTests
    {
        [Fact]
        public async Task GetExchangeRate_WithExistingCurrency_ReturnsRateWrappedInApiResponse()
        {
            // Arrange
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRateByCodeRequest { CurrencyCode = "USD" };

            // Act
            var result = await _sut.GetExchangeRate(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ExchangeRateResponse>>(okResult.Value);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal("USD", apiResponse.Data.SourceCurrency);
            Assert.Equal("CZK", apiResponse.Data.TargetCurrency);
            Assert.Equal(23.45m, apiResponse.Data.Rate);
        }

        [Fact]
        public async Task GetExchangeRate_WithCaseInsensitiveCurrency_ReturnsRate()
        {
            // Arrange
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRateByCodeRequest { CurrencyCode = "usd" };

            // Act
            var result = await _sut.GetExchangeRate(request, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetExchangeRate_WithNonExistentCurrency_Returns404()
        {
            // Arrange - Currency not in the provider's response
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRateByCodeRequest { CurrencyCode = "XYZ" };

            // Act
            var result = await _sut.GetExchangeRate(request, CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ExchangeRateResponse>>(notFound.Value);
            Assert.Equal(ErrorCodes.ExchangeRateCurrencyNotFound, apiResponse.ErrorCode);
            Assert.Null(apiResponse.Data);
        }

        [Fact]
        public async Task GetExchangeRate_WhenProviderFails_Returns502()
        {
            // Arrange
            var failure = Result<IReadOnlyCollection<ExchangeRate>>.Failure(
                ExchangeRateErrors.SourceUnavailable);

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(failure);

            var request = new GetExchangeRateByCodeRequest { CurrencyCode = "USD" };

            // Act
            var result = await _sut.GetExchangeRate(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status502BadGateway, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetExchangeRate_WithSpecificDate_PassesDateToProvider()
        {
            // Arrange
            var rates = SuccessRates(
                CreateRate("USD", "CZK", 23.45m));

            _provider
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(rates);

            var request = new GetExchangeRateByCodeRequest { CurrencyCode = "USD", Date = "2025-06-15" };

            // Act
            await _sut.GetExchangeRate(request, CancellationToken.None);

            // Assert
            await _provider.Received(1).GetDailyRatesAsync(
                new DateOnly(2025, 6, 15), Arg.Any<CancellationToken>());
        }
    }
}
