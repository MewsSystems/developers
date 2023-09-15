using FluentAssertions;
using Infrastructure.Models.Constants;
using Infrastructure.Models.CzechNationalBankModels;
using Infrastructure.Models.Exceptions;
using Infrastructure.Models.Responses;
using Infrastructure.Services.Tests.Unit.Fixture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Tests.Unit;

internal class ExchangeRateProviderTests
{
    
    private readonly IEnumerable<Currency> InputCurrencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR")
    };

    private readonly IEnumerable<Currency> InputCurrenciesLowercase = new[]
    {
            new Currency("usd"),
            new Currency("eur")
    };

    [Test]
    public async Task GetExchangeRates_ShouldReturnRatesForInputCurrencies()
    {
        var fixture = new ExchangeRateProviderFixture();

        var defaultCurrency = new Currency("CZK");
        var currencyRates = new List<CurrencyRateResponse>()
        {
            new CurrencyRateResponse()
            {
                Amount = 1,
                CurrencyCode = "USD",
                Rate = 1.5M
            },
            new CurrencyRateResponse()
            {
                Amount = 100,
                CurrencyCode = "EUR",
                Rate = 14.3M
            }
        };

        fixture.BankDataService
            .Setup(x => x.GetDefaultCurrency())
            .Returns(defaultCurrency);

        fixture.BankDataService
            .Setup(x => x.GetExchangeRates())
            .ReturnsAsync(currencyRates);

        var response = await fixture.CreateInstance().GetExchangeRates(InputCurrencies);

        response.Should().NotBeNull();
        response!.Count().Should().Be(currencyRates.Count);
        response.Should().BeEquivalentTo(new List<ExchangeRate>()
        {
            new ExchangeRate(new Currency("USD"), defaultCurrency, Math.Round(1/1.5M, RoundingConstants.NumberOfDecimalPlaces)),
            new ExchangeRate(new Currency("EUR"), defaultCurrency, Math.Round(100/14.3M, RoundingConstants.NumberOfDecimalPlaces))
        });
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnRatesForInputCurrenciesCaseInsensitive()
    {
        var fixture = new ExchangeRateProviderFixture();

        var defaultCurrency = new Currency("CZK");
        var currencyRates = new List<CurrencyRateResponse>()
        {
            new CurrencyRateResponse()
            {
                Amount = 1,
                CurrencyCode = "USD",
                Rate = 1.5M
            },
            new CurrencyRateResponse()
            {
                Amount = 100,
                CurrencyCode = "EUR",
                Rate = 14.3M
            }
        };

        fixture.BankDataService
            .Setup(x => x.GetDefaultCurrency())
            .Returns(defaultCurrency);

        fixture.BankDataService
            .Setup(x => x.GetExchangeRates())
            .ReturnsAsync(currencyRates);

        var response = await fixture.CreateInstance().GetExchangeRates(InputCurrenciesLowercase);

        response.Should().NotBeNull();
        response!.Count().Should().Be(currencyRates.Count);
        response.Should().BeEquivalentTo(new List<ExchangeRate>()
        {
            new ExchangeRate(new Currency("USD"), defaultCurrency, Math.Round(1/1.5M, RoundingConstants.NumberOfDecimalPlaces)),
            new ExchangeRate(new Currency("EUR"), defaultCurrency, Math.Round(100/14.3M, RoundingConstants.NumberOfDecimalPlaces))
        });
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnNullIfBankDataServiceDoesntReturnData()
    {
        var fixture = new ExchangeRateProviderFixture();

        fixture.BankDataService
            .Setup(x => x.GetExchangeRates())
            .ReturnsAsync((List<CurrencyRateResponse>)null!);

        var response = await fixture.CreateInstance().GetExchangeRates(InputCurrencies);

        response.Should().BeNull();
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnEmptyListIfInputCurrenciesDontMatchBankData()
    {
        var fixture = new ExchangeRateProviderFixture();

        var defaultCurrency = new Currency("CZK");
        var currencyRates = new List<CurrencyRateResponse>()
        {
            new CurrencyRateResponse()
            {
                Amount = 1,
                CurrencyCode = "ABC",
                Rate = 1.5M
            },
            new CurrencyRateResponse()
            {
                Amount = 100,
                CurrencyCode = "DEF",
                Rate = 14.3M
            }
        };

        fixture.BankDataService
            .Setup(x => x.GetDefaultCurrency())
            .Returns(defaultCurrency);

        fixture.BankDataService
            .Setup(x => x.GetExchangeRates())
            .ReturnsAsync(currencyRates);

        var response = await fixture.CreateInstance().GetExchangeRates(InputCurrencies);

        response.Should().NotBeNull();
        response!.Count().Should().Be(0);
        response.Should().BeEquivalentTo(new List<ExchangeRate>());
    }

    [Test]
    public async Task GetExchangeRates_ShoulThrowApiRequestExceptionIfBandDataServiceThrowsApiRequestException()
    {
        var fixture = new ExchangeRateProviderFixture();

        var defaultCurrency = new Currency("CZK");

        fixture.BankDataService
            .Setup(x => x.GetDefaultCurrency())
            .Returns(defaultCurrency);

        fixture.BankDataService
            .Setup(x => x.GetExchangeRates())
            .ThrowsAsync(new ApiRequestException());

        var action = async() => await fixture.CreateInstance().GetExchangeRates(InputCurrencies);

        await action.Should().ThrowAsync<ApiRequestException>();
    }
}
