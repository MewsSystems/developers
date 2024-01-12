using ExchangeRateProvider.Implementations.CzechNationalBank;
using ExchangeRateProvider.Implementations.CzechNationalBank.Models;
using ExchangeRateProvider.Models;
using NSubstitute;

namespace ExchangeRateProvider.Tests;

public class ExchangeRateProviderCzechNationalBankTests
{
    [Fact]
    public async Task GetExchangeRates_WhenTargetCurrencyExists_ThenReturnExchangeRate()
    {
        // arrange
        var targetCurrency = "AUD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = new ExRateDailyRest[]{
                new ExRateDailyRest() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = targetCurrency,
                    Order = 94,
                    Rate = 15.858m,
                    ValidFor = "2019-05-17"
                }
            }
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var targetCurrencies = new Currency[] {
            new (targetCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(targetCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.TargetCurrency.Code == targetCurrency);

        // assert
        Assert.True(exchangeRate != null, $"{targetCurrency} exchange rate should exist");
    }

    [Fact]
    public async Task GetExchangeRates_WhenTargetCurrencyDoesNotExist_ThenOmitExchangeRate()
    {
        // arrange
        var targetCurrency = "USD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = new ExRateDailyRest[]{
                new() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = "AUD",
                    Order = 94,
                    Rate = 15.858m,
                    ValidFor = "2019-05-17"
                }
            }
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var targetCurrencies = new Currency[] {
            new ("AUD"),
            new (targetCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(targetCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.TargetCurrency.Code == targetCurrency);

        // assert
        Assert.True(exchangeRate == null, $"{targetCurrency} exchange rate should be omitted");
    }

    [Fact]
    public async Task GetExchangeRates_WhenTranslatingApiResponse_ThenReturnCorrectValues()
    {
        // arrange
        var rate = 15.858m;
        var targetCurrency = "AUD";
        var bankApiMock = Substitute.For<ICzechNationalBankApi>();
        bankApiMock.GetExratesDaily(Arg.Any<DateTimeOffset>()).Returns(new ExRateDailyResponse
        {
            Rates = new ExRateDailyRest[]{
                new() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = targetCurrency,
                    Order = 94,
                    Rate = rate,
                    ValidFor = "2019-05-17"
                }
            }
        });
        var provider = new ExchangeRateProviderCzechNationalBank(bankApiMock);
        var targetCurrencies = new Currency[] {
            new (targetCurrency),
        };
        var date = new DateTimeOffset(2023, 12, 12, 0, 0, 0, TimeSpan.Zero);

        // act
        var exchangeRates = await provider.GetExchangeRates(targetCurrencies, date);
        var exchangeRate = exchangeRates.FirstOrDefault(x => x.TargetCurrency.Code == targetCurrency)!;

        // assert
        Assert.True(exchangeRate.SourceCurrency.Code == provider.SourceCurrency, $"SourceCurrency should be {provider.SourceCurrency}");
        Assert.True(exchangeRate.TargetCurrency.Code == targetCurrency, $"TargetCurrency should be {targetCurrency}");
        Assert.True(exchangeRate.Value == rate, $"Value should be {rate}");
    }
}