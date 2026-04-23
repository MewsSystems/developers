using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Application.Services;
using ExchangeRateUpdater.Api.Infrastructure.Interfaces;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Api.Tests.Application.Services;

public class ExchangeRateProviderTests
{
    private readonly IFixture _fixture;
    private readonly IExternalExchangeRateClient _externalClient;
    private readonly ExchangeRateProvider _sut;

    public ExchangeRateProviderTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _externalClient = _fixture.Freeze<IExternalExchangeRateClient>();
        var logger = _fixture.Freeze<ILogger<ExchangeRateProvider>>();
        _sut = new ExchangeRateProvider(_externalClient, logger);
    }

    public class GetDailyRatesAsync : ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetDailyRatesAsync_WithExternalRates_ReturnsMappedDomainModels()
        {
            // Arrange
            var response = new CnbExRateDailyResponse(
            [
                new CnbExRateDaily("2025-06-15", 115, "USA", "dollar", 1, "USD", 23.45m),
                new CnbExRateDaily("2025-06-15", 115, "EMU", "euro", 1, "EUR", 25.10m)
            ]);

            _externalClient
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(Result<CnbExRateDailyResponse>.Success(response));

            // Act
            var result = await _sut.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var rates = result.Value;
            Assert.Equal(2, rates.Count);
            Assert.All(rates, r => Assert.Equal("CZK", r.TargetCurrency.Code));
        }

        [Fact]
        public async Task GetDailyRatesAsync_MapsValidForFromExternalResponse()
        {
            // Arrange
            var response = new CnbExRateDailyResponse(
            [
                new CnbExRateDaily("2025-06-15", 115, "USA", "dollar", 1, "USD", 23.45m)
            ]);

            _externalClient
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(Result<CnbExRateDailyResponse>.Success(response));

            // Act
            var result = await _sut.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var rate = Assert.Single(result.Value);
            Assert.Equal(new DateOnly(2025, 6, 15), rate.ValidFor);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WithSpecificDate_PassesDateToExternalClient()
        {
            // Arrange
            var date = new DateOnly(2025, 6, 15);
            var response = new CnbExRateDailyResponse([]);

            _externalClient
                .GetDailyRatesAsync(date, Arg.Any<CancellationToken>())
                .Returns(Result<CnbExRateDailyResponse>.Success(response));

            // Act
            await _sut.GetDailyRatesAsync(date, CancellationToken.None);

            // Assert
            await _externalClient.Received(1).GetDailyRatesAsync(date, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetDailyRatesAsync_WithEmptyExternalResponse_ReturnsEmptyCollection()
        {
            // Arrange
            var response = new CnbExRateDailyResponse([]);

            _externalClient
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(Result<CnbExRateDailyResponse>.Success(response));

            // Act
            var result = await _sut.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WhenExternalClientFails_PropagatesError()
        {
            // Arrange
            var error = ExchangeRateErrors.SourceUnavailable;

            _externalClient
                .GetDailyRatesAsync(Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                .Returns(Result<CnbExRateDailyResponse>.Failure(error));

            // Act
            var result = await _sut.GetDailyRatesAsync(null, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(error.Code, result.Error.Code);
        }
    }
}
