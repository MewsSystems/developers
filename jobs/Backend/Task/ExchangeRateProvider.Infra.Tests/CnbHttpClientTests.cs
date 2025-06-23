using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Infra.Http;
using ExchangeRateUpdater.Infra.Models;
using Shouldly;
using Xunit;

namespace ExchangeRateProvider.Infra.Tests;

public class CnbHttpClientTests
{
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;
    private readonly CnbHttpClient _sut;

    public CnbHttpClientTests()
    {
        _mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("http://test.com");
        _sut = new CnbHttpClient(httpClient);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnFailureForErrorResponse()
    {
        // arrange
        _mockHttpMessageHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("Error response")
        });

        // act
        var result = await _sut.GetExchangeRatesAsync();
        
        // assert
        result.IsFailure.ShouldBe(true);
        await result.Match(
            response =>
            {
                response.ShouldBeNull();
                return Task.CompletedTask;
            },
            error =>
            {
                error.ShouldNotBeNull();
                error.HttpStatusCode.ShouldBe(HttpStatusCode.BadRequest);
                return Task.CompletedTask;
            });
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnSuccessForSuccessfulResponse()
    {
        // arrange
        _mockHttpMessageHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                "{\"rates\":[{\"validFor\":\"2019-05-17\",\"order\":94,\"country\":\"Australia\",\"currency\":\"dollar\",\"amount\":1,\"currencyCode\":\"AUD\",\"rate\":15.858},{\"validFor\":\"2019-05-17\",\"order\":94,\"country\":\"Brazil\",\"currency\":\"real\",\"amount\":1,\"currencyCode\":\"BRL\",\"rate\":5.686},{\"validFor\":\"2019-05-17\",\"order\":94,\"country\":\"Bulgaria\",\"currency\":\"lev\",\"amount\":1,\"currencyCode\":\"BGN\",\"rate\":13.162}]}")
        });

        // act
        var result = await _sut.GetExchangeRatesAsync();
        
        // assert
        result.IsSuccess.ShouldBe(true);
        await result.Match(
            response =>
            {
                response.ShouldNotBeNull();
                response.ShouldBeOfType<ExchangeRateResponse>();
                var rates = response.Rates.ToList();
                ;
                rates.ShouldNotBeNull();
                rates.Count.ShouldBe(3);
                return Task.CompletedTask;
            },
            error =>
            {
                error.ShouldBeNull();
                return Task.CompletedTask;
            });
    }
}