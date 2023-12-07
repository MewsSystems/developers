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
internal class ExchangeOrdersTests : ControllerTestBase
{
    [Test]
    public async Task GivenNoSourceCurrency_WhenMakingAExchangeOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "",
            TargetCurrency = "USD",
            SumToExchange = 10
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("Source Currency has to be specified.");
    }

    [Test]
    public async Task GivenNoTargetCurrency_WhenMakingAExchangeOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "",
            SumToExchange = 10
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("Target Currency has to be specified.");
    }

    [Test]
    public async Task GivenNullSumToExchange_WhenMakingAExchangeOrder_ShouldReturnBadResult()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = null
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("SumToExchange has to be specified.");
    }

    [Test]
    public async Task GivenMissingExchangePair_WhenMakingExchangeOrder_ShouldReturnNotFound()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = 100
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Be("We do not support exchange rates for the mentioned source/target currencies.");
    }

    [TestCase("CZK", "USD", 0.045, 1)]
    [TestCase("MDL", "USD", 0.056, 10)]
    [TestCase("EUR", "USD", 1.09, 2)]
    public async Task GivenValidExchangePair_WhenMakingExchangeOrder_ShouldReturnConvertedResult(string sourceCurrency, string targetCurrency, decimal rate, decimal sum)
    {
        // arrange
        var exchangeRate = new ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), new PositiveRealNumber(rate));
        ExchangeRateProviderRepository.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>(){
            exchangeRate
        });

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            SumToExchange = sum
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = JsonConvert.DeserializeObject<ExchangeResultDto>(await response.Content.ReadAsStringAsync());
        responseContent.Should().BeEquivalentTo(new ExchangeResultDto
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            ConvertedSum = sum * rate
        });
    }

    [Test]
    public async Task GivenMultipleExchangePair_WhenMakingAExchangeOrder_ShouldReturnConvertedResultWithCorrectPair()
    {
        // arrange
        var correctExchangePair = new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(0.045m));
        var wrongExchangePair = new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(0.056m));
        ExchangeRateProviderRepository.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>{
            wrongExchangePair,
            correctExchangePair
        });

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange");
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = 100
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = JsonConvert.DeserializeObject<ExchangeResultDto>(await response.Content.ReadAsStringAsync());
        responseContent.Should().BeEquivalentTo(new ExchangeResultDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            ConvertedSum = 100 * 0.045m
        });
    }


    [Test]
    public async Task GivenExchangePairWithDifferenDates_WhenMakingAExchangeOrder_ShouldReturnConvertedResultWithCorrectPairBeforeOrEqualToRequestedDate()
    {
        // arrange
        var wrongExchangePair = new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(0.056m));
        ExchangeRateProviderRepository.UpsertExchangeRate(DateTime.Now.AddDays(-2), new HashSet<ExchangeRate>{
            wrongExchangePair,
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(0.045m))
        });

        ExchangeRateProviderRepository.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>{
            wrongExchangePair,
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(0.048m))
        });

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange").SetQueryParam("requestDate", DateTime.Now.AddDays(-1));
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            SumToExchange = 100
        };
        var content = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = JsonConvert.DeserializeObject<ExchangeResultDto>(await response.Content.ReadAsStringAsync());
        responseContent.Should().BeEquivalentTo(new ExchangeResultDto
        {
            SourceCurrency = "CZK",
            TargetCurrency = "USD",
            ConvertedSum = 100 * 0.045m
        });
    }
}
