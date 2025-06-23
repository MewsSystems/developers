using System;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Console;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ExchangeRateUpdater.IntegrationTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnExpectedRatesWhenSuccessfulResponse()
    {
        // arrange
        var port = PortUtils.FindFreePort();
        var baseUrl = $"http://cnbapi.localtest.me:{port}";
        using var server = WireMockServer.Start(port);
        Environment.SetEnvironmentVariable("CnbClient__BaseUrl", baseUrl);
        var successResponse = ResourceReader.Read("Responses.SuccessfulResponse.json");
        server.Given(
                Request.Create().WithUrl($"{baseUrl}/exrates/daily?lang=EN")
                    .UsingGet())
            .RespondWith(Response.Create()
                .WithBody(successResponse)
                .WithStatusCode(200));
        var serviceProvider = Program.BuildServiceProvider();
        var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        
        // act
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeEmpty();
        rateList.Count.ShouldBe(6);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnEmptyListWhenNonTransientErrorResponse()
    {
        // arrange
        var port = PortUtils.FindFreePort();
        var baseUrl = $"http://cnbapi.localtest.me:{port}";
        using var server = WireMockServer.Start(port);
        Environment.SetEnvironmentVariable("CnbClient__BaseUrl", baseUrl);
        server.Given(
                Request.Create().WithUrl($"{baseUrl}/exrates/daily?lang=EN")
                    .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(400));
        var serviceProvider = Program.BuildServiceProvider();
        var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        
        // act
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnExpectedRatesWhenShortStandingTransientError()
    {
        // arrange
        var port = PortUtils.FindFreePort();
        var baseUrl = $"http://cnbapi.localtest.me:{port}";
        using var server = WireMockServer.Start(port);
        Environment.SetEnvironmentVariable("CnbClient__BaseUrl", baseUrl);
        var successResponse = ResourceReader.Read("Responses.SuccessfulResponse.json");
        server.Given(
                Request.Create().WithUrl($"{baseUrl}/exrates/daily?lang=EN")
                    .UsingGet())
            .InScenario("Transient Error")
            .WillSetStateTo("500 response")
            .RespondWith(Response.Create()
                .WithStatusCode(500));
        server.Given(
                Request.Create().WithUrl($"{baseUrl}/exrates/daily?lang=EN")
                    .UsingGet())
            .InScenario("Transient Error")
            .WhenStateIs("500 response")
            .WillSetStateTo("Valid response")
            .RespondWith(Response.Create()
                .WithBody(successResponse)
                .WithStatusCode(200));
        var serviceProvider = Program.BuildServiceProvider();
        var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        
        // act
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldNotBeEmpty();
        rateList.Count.ShouldBe(6);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsyncShouldReturnEmptyListWhenLongStandingTransientErrorResponse()
    {
        // arrange
        var port = PortUtils.FindFreePort();
        var baseUrl = $"http://cnbapi.localtest.me:{port}";
        using var server = WireMockServer.Start(port);
        Environment.SetEnvironmentVariable("CnbClient__BaseUrl", baseUrl);
        server.Given(
                Request.Create().WithUrl($"{baseUrl}/exrates/daily?lang=EN")
                    .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(500));
        var serviceProvider = Program.BuildServiceProvider();
        var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        
        // act
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // assert
        var rateList = rates.ToList();
        rateList.ShouldBeEmpty();
    }

}