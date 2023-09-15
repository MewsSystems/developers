using FluentAssertions;
using Infrastructure.Models.AppSettings;
using Infrastructure.Models.CzechNationalBankModels;
using Infrastructure.Models.Exceptions;
using Infrastructure.Models.Responses;
using Infrastructure.Services.Tests.Unit.Fixture;
using Infrastructure.Services.Tests.Unit.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.Tests.Unit;

internal class CzechNationalBankDataServiceTests
{
    [Test]
    public void GetDefaultCurrency_ShouldReturnCzk()
    {
        var fixture = new CzechNationalBankDataServiceFixture();

        fixture.Options
            .Setup(x => x.Value)
            .Returns(new CzechNationalBankSettings
            {
                DefaultCurrencyCode = "CZK"
            });

        var response = fixture.CreateInstance().GetDefaultCurrency();

        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(new Currency("CZK"));
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnExchangeRates()
    {
        var fixture = new CzechNationalBankDataServiceFixture();

        var expectedResponse = new ExchangeRateDailyResponse()
        {
            Rates = new List<CurrencyRateResponse>
            {
                new CurrencyRateResponse
                {
                    CurrencyCode = "USD",
                    Amount = 1,
                    Rate = 23.453M
                },
                new CurrencyRateResponse
                {
                     CurrencyCode = "EUR",
                     Amount = 1,
                     Rate = 24.71M
                }
            }
        };

        var httpClient = HttpClientTestHelper.CreateTestClient(JsonSerializer.Serialize(expectedResponse), HttpStatusCode.OK);

        var response = await fixture.CreateInstance(httpClient).GetExchangeRates();

        response.Should().NotBeNull();
        response!.Count.Should().Be(expectedResponse.Rates.Count);
        response!.Should().BeEquivalentTo(expectedResponse.Rates);
    }

    [Test]
    public async Task GetExchangeRates_ShouldThrowApiRequestExceptionForUnsuccessfulRequest()
    {
        var fixture = new CzechNationalBankDataServiceFixture();

        var httpClient = HttpClientTestHelper.CreateTestClient("", HttpStatusCode.BadRequest);

        var action = async () => await fixture.CreateInstance(httpClient).GetExchangeRates();

        await action.Should().ThrowAsync<ApiRequestException>();
        fixture.Logger.VerifyLog(
            LogLevel.Error,
            Times.Exactly(1));
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnNullForEmptyContent()
    {
        var fixture = new CzechNationalBankDataServiceFixture();

        var httpClient = HttpClientTestHelper.CreateTestClient("{}", HttpStatusCode.OK);

        var response = await fixture.CreateInstance(httpClient).GetExchangeRates();
        
        response.Should().BeNull();
        fixture.Logger.VerifyLog(
                LogLevel.Information,
                Times.Exactly(1));
    }
}