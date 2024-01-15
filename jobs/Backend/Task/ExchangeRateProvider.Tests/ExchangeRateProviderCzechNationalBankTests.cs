using ExchangeRateProvider.Implementations.CzechNationalBank;
using ExchangeRateProvider.Implementations.CzechNationalBank.Models;
using ExchangeRateProvider.Models;
using NSubstitute;

namespace ExchangeRateProvider.Tests;

public class ExchangeRateProviderCzechNationalBankTests
{
    [Fact]
    public async Task GetExchangeRates_WhenSourceCurrencyExists_ThenReturnExchangeRate()
    {
        // arrange
        var sourceCurrency = "AUD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = [
                new() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = sourceCurrency,
                    Order = 94,
                    Rate = 15.858m,
                    ValidFor = "2019-05-17"
                }
            ]
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var sourceCurrencies = new Currency[] {
            new(sourceCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(sourceCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.SourceCurrency.Code == sourceCurrency);

        // assert
        Assert.True(exchangeRate != null, $"{sourceCurrency} exchange rate should exist");
    }

    [Fact]
    public async Task GetExchangeRates_WhenSourceCurrencyDoesNotExist_ThenOmitExchangeRate()
    {
        // arrange
        var sourceCurrency = "USD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = [
                new() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = "AUD",
                    Order = 94,
                    Rate = 15.858m,
                    ValidFor = "2019-05-17"
                }
            ]
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var sourceCurrencies = new Currency[] {
            new("AUD"),
            new(sourceCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(sourceCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.SourceCurrency.Code == sourceCurrency);

        // assert
        Assert.True(exchangeRate == null, $"{sourceCurrency} exchange rate should be omitted");
    }

    [Fact]
    public async Task GetExchangeRates_WhenTranslatingApiResponse_ThenReturnCorrectValues()
    {
        // arrange
        var rate = 15.858m;
        var sourceCurrency = "AUD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = [
                new() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = sourceCurrency,
                    Order = 94,
                    Rate = rate,
                    ValidFor = "2019-05-17"
                }
            ]
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var sourceCurrencies = new Currency[] {
            new(sourceCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(sourceCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.SourceCurrency.Code == sourceCurrency)!;

        // assert
        Assert.True(exchangeRate.TargetCurrency.Code == provider.TargetCurrency, $"TargetCurrency should be {provider.TargetCurrency}");
        Assert.True(exchangeRate.SourceCurrency.Code == sourceCurrency, $"SourceCurrency should be {sourceCurrency}");
        Assert.True(exchangeRate.Value == rate, $"Value should be {rate}");
    }
}