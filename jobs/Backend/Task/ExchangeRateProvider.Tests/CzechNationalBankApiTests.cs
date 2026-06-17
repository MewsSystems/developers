using System.Net;
using ExchangeRateProvider.Exceptions;
using ExchangeRateProvider.Implementations;
using ExchangeRateProvider.Implementations.CzechNationalBank;
using ExchangeRateProvider.Implementations.CzechNationalBank.Models;
using NSubstitute;

namespace ExchangeRateProvider.Tests;

public class CzechNationalBankApiTests
{
    private const string GET_EXRATES_DAILY_VALID_RESPONSE_BODY = @"
    {
        ""rates"": [
        {
            ""validFor"": ""2019-05-17"",
            ""order"": 94,
            ""country"": ""Australia"",
            ""currency"": ""dollar"",
            ""amount"": 1,
            ""currencyCode"": ""AUD"",
            ""rate"": 15.858
        }
        ]
    }";

    [Fact]
    public async Task GetExratesDaily_WhenCreatingUrl_ThenReturnCorrectUrlFormat()
    {
        // arrange
        var httpClientMock = Substitute.For<IHttpClient>();
        httpClientMock.Get(Arg.Any<string>()).Returns(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(GET_EXRATES_DAILY_VALID_RESPONSE_BODY),
        });
        var bankApi = new CzechNationalBankApi(httpClientMock);
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        await bankApi.GetExratesDaily(date);

        // assert
        await httpClientMock.Received().Get("cnbapi/exrates/daily?date=2023-12-12&lang=EN");
    }

    [Fact]
    public async Task ProcessHttpResponse_WhenBodyIsEmpty_ThenThrowUnexpectedError()
    {
        // arrange
        var httpClientMock = Substitute.For<IHttpClient>();
        var httpResponseMsg = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(""),
        };
        var bankApi = new CzechNationalBankApi(httpClientMock);

        // act / assert
        await Assert.ThrowsAsync<UnexpectedException>(() =>
            bankApi.ProcessHttpResponse<ExRateDailyResponse>(httpResponseMsg)
        );
    }

    [Fact]
    public async Task ProcessHttpResponse_WhenStatusCodeIsError_ThenThrowUnexpectedError()
    {
        // arrange
        var httpClientMock = Substitute.For<IHttpClient>();
        var httpResponseMsg = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(GET_EXRATES_DAILY_VALID_RESPONSE_BODY),
        };
        var bankApi = new CzechNationalBankApi(httpClientMock);

        // act / assert
        await Assert.ThrowsAsync<UnexpectedException>(() =>
            bankApi.ProcessHttpResponse<ExRateDailyResponse>(httpResponseMsg)
        );
    }

    [Fact]
    public async Task ProcessHttpResponse_WhenBodyCantBeParsed_ThenThrowUnexpectedError()
    {
        // arrange
        var httpClientMock = Substitute.For<IHttpClient>();
        var httpResponseMsg = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""field"": 2}"),
        };
        var bankApi = new CzechNationalBankApi(httpClientMock);

        // act / assert
        await Assert.ThrowsAsync<UnexpectedException>(() =>
            bankApi.ProcessHttpResponse<ExRateDailyResponse>(httpResponseMsg)
        );
    }
}