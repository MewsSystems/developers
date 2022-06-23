using System.Globalization;
using System.Net;
using ExchangeRateUpdater.Service.Cnb;

namespace ExchangeRateUpdater.Service.Tests.Unit.Tests;

public class ExchangeRateServiceTests
{
    [Test]
    public async Task GivenCorrectData_GetExchangeRates_ShouldReturnCorrectAmountOfData()
    {
        //Arrange
        var testRate = A.TestRateValue.ToString(new NumberFormatInfo { CurrencyDecimalSeparator = A.MappingDecimalSeparator });
        var correctStream = new ExchangeRateStreamBuilder().WithTitle($"{A.TestDate:dd MMM yyyy} #106")
                                                           .WithColumnInfo("Country|Currency|Amount|Code|Rate")
                                                           .WithExchangeRate($"NL|Source|1|{A.TestSourceCurrency.Code}|{testRate}")
                                                           .Build();

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content    = new StreamContent(correctStream)
        };

        var exchangeRateService = new CnbService(TestHelper.HttpClientWithResponse(httpResponse),
                                                 new ExchangeRateTestSettings(),
                                                 new LoggerNull());

        //Act
        var response = (await exchangeRateService.GetExchangeRatesAsync(new DateTime(2022, 06, 01))).ToList()!;

        //Assert
        response.Should().NotBeNullOrEmpty();
        response.Single().Value.Should().Be(A.TestRateValue);
        response.Single().SourceCurrency.ToString().Should().Be(A.TestSourceCurrency.Code);
        response.Single().TargetCurrency.ToString().Should().Be(A.TestTargetCurrency.Code);
    }

    [Test]
    public async Task GivenIncorrectData_GetExchangeRates_ShouldThrowException()
    {
        //Arrange
        var incorrectStream = new ExchangeRateStreamBuilder().WithTitle($"{A.TestDate:dd MMM yyyy} #106")
                                                             .WithColumnInfo("Country|Currency|Amount|Code|Rate")
                                                             .WithExchangeRate($"NL|Source|this-should-not-be-string||")
                                                             .WithExchangeRate($"NL|Source|this-should-not-be-string||")
                                                             .Build();

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content    = new StreamContent(incorrectStream)
        };

        var exchangeRateService = new CnbService(TestHelper.HttpClientWithResponse(httpResponse),
                                                 new ExchangeRateTestSettings(),
                                                 new LoggerNull());

        //Act
        Func<Task> act = () => exchangeRateService.GetExchangeRatesAsync(new DateTime(2022, 06, 01));

        //Assert
        await act.Should().ThrowAsync<Exception>("throwExceptionOnError parameter is true");
    }
}