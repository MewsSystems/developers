using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.ExchangeRateControllerTests;

[TestFixture]
internal class BuyOrdersTests : ControllerTestBase
{
    [Test]
    public async Task GivenNoSourceCurrency_WhenMakingABuyOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = "",
            TargetCurrency = "USD",
            SumToExchange = 10
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("Source Currency has to be specified.");
    }

    [Test]
    public async Task GivenNoTargetCurrency_WhenMakingABuyOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "",
            SumToExchange = 10
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("Target Currency has to be specified.");
    }

    [Test]
    public async Task GivenNullSumToExchange_WhenMakingABuyOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = null
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("SumToExchange has to be specified.");
    }

    [Test]
    public async Task GivenMissingExchangePair_WhenMakingABuyOrder_ShouldReturnNotFound()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = 100
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("We do not support exchange rates for the mentioned source/target currencies.");
    }

    [TestCase("CZK", "USD", 0.045, 1)]
    [TestCase("MDL", "USD", 0.056, 10)]
    [TestCase("EUR", "USD", 1.09, 2)]
    public async Task GivenValidExchangePair_WhenMakingABuyOrder_ShouldReturnConvertedResult(string sourceCurrency, string targetCurrency, decimal rate, decimal sum)
    {
        // arrange
        var exchangeRate = new ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), new PositiveRealNumber(rate));
        ExchangeRateProviderRepository.UpsertExchangeRate(exchangeRate);

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            SumToExchange = sum
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = JsonConvert.DeserializeObject<BuyResultDto>(await response.Content.ReadAsStringAsync());
        responseContent.Should().BeEquivalentTo(new BuyResultDto
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            ConvertedSum = sum * rate
        });
    }

    [Test]
    public async Task GivenMultipleExchangePair_WhenMakingABuyOrder_ShouldReturnConvertedResultWithCorrectPair()
    {
        // arrange
        var correctExchangePair = new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(0.045m));
        var wrongExchangePair = new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(0.056m));
        ExchangeRateProviderRepository.UpsertExchangeRate(wrongExchangePair);
        ExchangeRateProviderRepository.UpsertExchangeRate(correctExchangePair);

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var buyOrderDto = new BuyOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = 100
        };
        var content = new StringContent(JsonConvert.SerializeObject(buyOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = JsonConvert.DeserializeObject<BuyResultDto>(await response.Content.ReadAsStringAsync());
        responseContent.Should().BeEquivalentTo(new BuyResultDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            ConvertedSum = 100 * 0.045m
        });
    }
}
