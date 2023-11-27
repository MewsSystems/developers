using System.Net;
using System.Text;
using Domain.Entities;
using Domain.ValueTypes;
using ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;
using ExchangeRateUpdaterApi.Tests.Unit.Dtos.Response;
using ExchangeRateUpdaterApi.Tests.Unit.TestHelpers.Builders;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
using ExchangeRatesRequestDto = ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request.ExchangeRatesRequestDto;

namespace ExchangeRateUpdaterApi.Tests.Unit.Controllers;

[TestFixture]
public class ExchangeRatesControllerTests : TestBase
{
    [Test]
    public async Task GivenNoExchangeRatesToRequest_ShouldReturnBadRequest()
    {
        // arrange
        var date = new DateTime(2023, 11, 25);
        
        var exchangeRatesToRequest = new ExchangeRatesRequestDto();
        
        // act
        var body = JsonConvert.SerializeObject(exchangeRatesToRequest);

        var requestUrl = "api/exchangerates"
            .AppendPathSegment("getExchangeRates")
            .SetQueryParam("date", date);

        HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(requestUrl, content);

        // assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GivenOneExchangeRateToRequest_WhenExchangeRateIsNotAvailable_ShouldReturnEmptyList()
    {
        // arrange
        var date = new DateTime(2023, 11, 25);
        
        var exchangeRatesToRequest = new ExchangeRatesRequestDto();
        var exchangeRate = new ExchangeRateRequestDetailsDtoBuilder()
            .WithSourceCurrency(new CurrencyDto("USD"))
            .WithTargetCurrency(new CurrencyDto("CZK"))
            .Build();
        
        exchangeRatesToRequest.AddExchangeRate(exchangeRate);
        
        // act
        var body = JsonConvert.SerializeObject(exchangeRatesToRequest);
        
        var requestUrl = "api/exchangerates"
            .AppendPathSegment("getExchangeRates")
            .SetQueryParam("date", date);

        HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(requestUrl, content);
        
        // assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var getExchangeRatesResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        var getExchangeRatesResult = JsonConvert.DeserializeObject<List<ExchangeRateResultDto>>(getExchangeRatesResponse);

        getExchangeRatesResult.Should().BeEmpty();
    }

    [Test]
    public async Task GivenOneExchangeRateToRequest_WhenExchangeRateIsPresent_ShouldReturnExchangeRate()
    {
        // arrange
        var date = new DateTime(2023, 11, 25);
        
        var exchangeRatesToRequest = new ExchangeRatesRequestDto();
        var exchangeRateRequest = new ExchangeRateRequestDetailsDtoBuilder()
            .WithSourceCurrency(new CurrencyDto("USD"))
            .WithTargetCurrency(new CurrencyDto("CZK"))
            .Build();
        
        exchangeRatesToRequest.AddExchangeRate(exchangeRateRequest);
        
        var exchangeRate = new ExchangeRate(
            new Currency("USD"),
            new Currency("CZK"),
            new decimal(22.00)
        );
        
        ExchangeRatesRepositoryInMemory.AddExchangeRate(exchangeRate, date);
        
        // act
        var body = JsonConvert.SerializeObject(exchangeRatesToRequest);

        var requestUrl = "api/exchangerates"
            .AppendPathSegment("getExchangeRates")
            .SetQueryParam("date", date);

        HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(requestUrl, content);
        
        // assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var getExchangeRatesResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        var getExchangeRatesResult = JsonConvert.DeserializeObject<List<ExchangeRateResultDto>>(getExchangeRatesResponse);

        getExchangeRatesResult.Should().NotBeEmpty();
        getExchangeRatesResult.Should().BeEquivalentTo(
            new List<ExchangeRateResultDto>
            {
                new ExchangeRateResultDto
                {
                    SourceCurrency = new CurrencyDto("USD"),
                    TargetCurrency = new CurrencyDto("CZK"),
                    Value = new decimal(22.00)
                }
            }
        );
    }
    
    [Test]
    public async Task GivenOneExchangeRateToRequest_WhenMultipleExchangeRatesArePresent_ShouldReturnRequestedExchangeRate()
    {
        // arrange
        var date = new DateTime(2023, 11, 25);
        
        var exchangeRatesToRequest = new ExchangeRatesRequestDto();
        var exchangeRateRequest = new ExchangeRateRequestDetailsDtoBuilder()
            .WithSourceCurrency(new CurrencyDto("USD"))
            .WithTargetCurrency(new CurrencyDto("CZK"))
            .Build();
        
        exchangeRatesToRequest.AddExchangeRate(exchangeRateRequest);
        
        var exchangeRate1 = new ExchangeRate(
            new Currency("USD"),
            new Currency("CZK"),
            new decimal(22.00)
        );

        var exchangeRate2 = new ExchangeRate(
            new Currency("CZK"),
            new Currency("USD"),
            new decimal(12.00)
        );
        
        ExchangeRatesRepositoryInMemory.AddExchangeRate(exchangeRate1, date);
        ExchangeRatesRepositoryInMemory.AddExchangeRate(exchangeRate2, date);
        
        // act
        var body = JsonConvert.SerializeObject(exchangeRatesToRequest);

        var requestUrl = "api/exchangerates"
            .AppendPathSegment("getExchangeRates")
            .SetQueryParam("date", date);

        HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(requestUrl, content);
        
        // assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        var getExchangeRatesResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        var getExchangeRatesResult = JsonConvert.DeserializeObject<List<ExchangeRateResultDto>>(getExchangeRatesResponse);

        getExchangeRatesResult.Should().NotBeEmpty();
        getExchangeRatesResult.Count.Should().Be(1);
        getExchangeRatesResult.Should().BeEquivalentTo(
            new List<ExchangeRateResultDto>
            {
                new ExchangeRateResultDto
                {
                    SourceCurrency = new CurrencyDto("USD"),
                    TargetCurrency = new CurrencyDto("CZK"),
                    Value = new decimal(22.00)
                }
            }
        );
    }
}