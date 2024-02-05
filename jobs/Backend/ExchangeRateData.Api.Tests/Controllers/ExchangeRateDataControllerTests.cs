using AutoFixture;
using ExchangeRateData.Api.Controllers;
using ExchangeRateData.DataConnectors.Models;
using ExchangeRateData.DataConnectors.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class ExchangeRateDataControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IExchangeRateRepository> _exchangeRateRepositoryMock;
    private readonly Mock<ILogger<ExchangeRateDataController>> _loggerMock;
    private readonly ExchangeRateDataController _controller;

    public ExchangeRateDataControllerTests()
    {
        _fixture = new Fixture();
        _exchangeRateRepositoryMock = _fixture.Freeze<Mock<IExchangeRateRepository>>();
        _loggerMock = _fixture.Freeze<Mock<ILogger<ExchangeRateDataController>>>();
        _controller = new ExchangeRateDataController(_loggerMock.Object, _exchangeRateRepositoryMock.Object);

    }

    [Fact]
    public async Task GetExchangeRateDataByDate_ShouldReturnNotFound_WhenDataNotFound()
    {
        //arrange
        IEnumerable<ExchangeRate>? response = null;
        string dateSubmitted = "04.02.2024";
        string[] invalidCurrencies = new string[] { "CZK" };
        _exchangeRateRepositoryMock.Setup(x => x.GetExchangeRatesByCurrencyAndDateTaskAsync(dateSubmitted, invalidCurrencies)).ReturnsAsync(response);

        //act
        var result = await _controller.GetExchangeRatesByCurrencyAndDate(selectedDate: dateSubmitted, currencies: invalidCurrencies).ConfigureAwait(false);

        //assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<NotFoundResult>();
        _exchangeRateRepositoryMock.Verify(x => x.GetExchangeRatesByCurrencyAndDateTaskAsync(dateSubmitted, invalidCurrencies), Times.Once());
    }

    [Fact]
    public async Task GetExchangeRateDataByDate_ShouldReturnBadRequest_WhenInvalidInput()
    {
        //arrange
        var exchangeRootMock = _fixture.Create<IEnumerable<ExchangeRate>>();
        string invalidDateSubmitted = "04/02/2024";
        string[] invalidCurrencies = new string[] { "CZK" };
        _exchangeRateRepositoryMock.Setup(x => x.GetExchangeRatesByCurrencyAndDateTaskAsync(invalidDateSubmitted, invalidCurrencies));

        //act
        var result = await _controller.GetExchangeRatesByCurrencyAndDate(selectedDate: invalidDateSubmitted, currencies: invalidCurrencies).ConfigureAwait(false);

        //assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<BadRequestResult>();
        _exchangeRateRepositoryMock.Verify(x => x.GetExchangeRatesByCurrencyAndDateTaskAsync(invalidDateSubmitted, invalidCurrencies), Times.Never());
    }

}