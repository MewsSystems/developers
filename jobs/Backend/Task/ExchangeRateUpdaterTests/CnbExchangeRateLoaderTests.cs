using System.Text;
using System.Text.Json;
using AutoFixture;
using AutoFixture.Xunit2;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.DataSource.Cnb;
using ExchangeRateUpdater.DataSource.Cnb.Dto;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests;

public class CnbExchangeRateLoaderTests
{
    private static readonly DateTime Today = new(2025, 11, 09);

    [Theory, LocalData]
    internal async Task ThrowExceptionInCaseOfNullCurrenciesParam(CnbExchangeRateLoader loader)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => loader.GetExchangeRatesAsync(null, Today));
    }

    [Theory, LocalData]
    internal async Task ThrowExceptionIfDateIsInFuture(CnbExchangeRateLoader loader)
    {
        var tomorrow = Today.AddDays(1);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => loader.GetExchangeRatesAsync([], tomorrow));

        Assert.Equal("The date should not be in the future (Parameter 'date')", ex.Message);
    }

    [Theory, LocalData]
    internal async Task ReturnEmptyListIfNoCurrenciesWasRequested(CnbExchangeRateLoader loader)
    {
        var currencies = new string[0];
        var rates = await loader.GetExchangeRatesAsync(currencies, Today);

        Assert.Empty(rates);
    }

    [Theory, LocalData]
    internal void ThrowExceptionIfUriIsInvalid(
        Mock<IRequestHandler> requestHandlerMock,
        Mock<ILogger<CnbExchangeRateLoader>> loggerMock,
        Mock<IRefreshScheduleFactory> factoryMock,
        CnbRateConverter rateConverter,
        Mock<IDateTimeService> dateTimeServiceMock)
    {
        var incorrectUrl = "tratata";
        var config = new ExchangeRateLoaderConfig() { BaseApiUrl = incorrectUrl, RefreshScheduleConfig = null };

        Assert.Throws<UriFormatException>(
            () => new CnbExchangeRateLoader(requestHandlerMock.Object, rateConverter, loggerMock.Object, factoryMock.Object, dateTimeServiceMock.Object, config));
    }

    [Theory, LocalData]
    internal async Task CanLoadRates(
        Mock<IRequestHandler> requestHandlerMock,
        CnbExchangeRateLoader loader)
    {
        var response = new CnbRateResponse()
        {
            Rates =
            [
                new CnbRate() { CurrencyCode = IsoCurrencyCode.EUR.ToString(), Amount = 1, Rate = 24.335M },
                new CnbRate() { CurrencyCode = IsoCurrencyCode.JPY.ToString(), Amount = 100, Rate = 14.11M },
                new CnbRate() { CurrencyCode = IsoCurrencyCode.TRY.ToString(), Amount = 100, Rate = 5.22M }
            ]
        };
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)));
        requestHandlerMock.Setup(h => h.GetStreamAsync(It.IsAny<string>())).Returns(Task.FromResult<Stream>(stream));

        var result = (await loader.GetExchangeRatesAsync(["EUR", "JPY"], Today)).ToArray();

        Assert.Equal(2, result.Length);
        var eurRate = result.Single(r => r.SourceCurrency.Code == IsoCurrencyCode.EUR);
        Assert.Equal(IsoCurrencyCode.EUR, eurRate.SourceCurrency.Code);
        Assert.Equal(IsoCurrencyCode.CZK, eurRate.TargetCurrency.Code);
        Assert.Equal(24.335M, eurRate.Value);

        var jpyRate = result.Single(r => r.SourceCurrency.Code == IsoCurrencyCode.JPY);
        Assert.Equal(IsoCurrencyCode.JPY, jpyRate.SourceCurrency.Code);
        Assert.Equal(IsoCurrencyCode.CZK, jpyRate.TargetCurrency.Code);
        Assert.Equal(0.1411M, jpyRate.Value);
    }
    
    [Theory, LocalData]
    internal async Task DoNotReturnNonExistentCurrency(
        Mock<IRequestHandler> requestHandlerMock,
        CnbExchangeRateLoader loader)
    {
        var response = new CnbRateResponse() 
        {
            Rates =
            [
                new CnbRate() { CurrencyCode = IsoCurrencyCode.EUR.ToString(), Amount = 1, Rate = 24.335M },
                new CnbRate() { CurrencyCode = "XYZ", Amount = 1, Rate = 15M }
            ] 
            };
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)));
        requestHandlerMock.Setup(h => h.GetStreamAsync(It.IsAny<string>())).Returns(Task.FromResult<Stream>(stream));

        var result = (await loader.GetExchangeRatesAsync(["XYZ", "EUR"], Today)).ToArray();

        Assert.Single(result);
        Assert.Equal(IsoCurrencyCode.EUR, result[0].SourceCurrency.Code);
        Assert.Equal(IsoCurrencyCode.CZK, result[0].TargetCurrency.Code);
        Assert.Equal(24.335M, result[0].Value);
    }

    private class LocalDataAttribute : AutoDataAttribute
    {
        public LocalDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            var scheduleConfig = new RefreshScheduleConfig() { Time = "14:30", TimeZone = "Central European Standard Time" };
            var config = new ExchangeRateLoaderConfig() { BaseApiUrl = "https://api.tratata", RefreshScheduleConfig = scheduleConfig };

            var loaderLoggerMock = new Mock<ILogger<CnbExchangeRateLoader>>();
            var converterLoggerMock = new Mock<ILogger<CnbRateConverter>>();

            var requestHandlerMock = new Mock<IRequestHandler>();

            var refreshScheduleFactoryMock = new Mock<IRefreshScheduleFactory>();
            refreshScheduleFactoryMock.Setup(f => f.CreateRefreshSchedule(scheduleConfig)).Returns(new Mock<IRateRefreshScheduler>().Object);

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            dateTimeServiceMock.Setup(s => s.GetToday()).Returns(Today);

            var rateConverter = new CnbRateConverter(converterLoggerMock.Object);

            var loader = new CnbExchangeRateLoader(requestHandlerMock.Object, rateConverter, loaderLoggerMock.Object, refreshScheduleFactoryMock.Object, dateTimeServiceMock.Object, config);

            fixture.Inject(requestHandlerMock);
            fixture.Inject(loaderLoggerMock);
            fixture.Inject(refreshScheduleFactoryMock);
            fixture.Inject(dateTimeServiceMock);
            fixture.Inject(rateConverter);
            fixture.Inject(loader);

            return fixture;
        }
    }
}
