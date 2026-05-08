using ExchangeRateProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;

namespace ExchangeRateProvider.Tests;

public class ExchangeRateProviderTests
{
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    [Fact]
    public async Task When_provider_is_called_with_currencies_not_available_in_bank_api_Then_provider_should_ignore_them()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var bankApiClient = new Mock<IBankApiClient>();
        var logger = new Mock<ILogger<IExchangeRateProvider>>();

        bankApiClient.Setup(client =>
            client.GetDailyExchangeRatesAsync(null, default))
                .Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
                    [
                        new BankCurrencyRate(1, "EUR", 24.2m)
                    ]
                ));

        var provider = new ExchangeRateProvider(bankApiClient.Object, cache, logger.Object, _timeProvider);

        // Act
        var exchangeRates = await provider.GetExchangeRatesAsync(
            new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c)));

        // Assert
        exchangeRates.ShouldNotBeNull();
        exchangeRates.ShouldHaveSingleItem();
        exchangeRates.ShouldContain(
            r =>
                r.SourceCurrency.Code == "EUR" &&
                r.Value == 24.2m);
    }

    [Fact]
    public async Task When_bank_api_rate_has_amount_greater_than_one_Then_provider_should_divide_the_rate_by_the_amount()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var bankApiClient = new Mock<IBankApiClient>();
        var logger = new Mock<ILogger<IExchangeRateProvider>>();
        const decimal currencyRate = 14.528m;
        const long currencyAmount = 100L;

        bankApiClient.Setup(client =>
                client.GetDailyExchangeRatesAsync(null, default))
            .Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
                [
                    new BankCurrencyRate(currencyAmount, "JPY", currencyRate)
                ]
            ));

        var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
        var provider = new ExchangeRateProvider(bankApiClient.Object, cache, logger.Object, _timeProvider);

        // Act
        var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        exchangeRates.ShouldNotBeNull();
        exchangeRates.ShouldHaveSingleItem();
        exchangeRates.ShouldContain(
            r =>
                r.SourceCurrency.Code == "JPY" &&
                r.Value == decimal.Divide(currencyRate, currencyAmount));
    }

    [Fact]
    public async Task When_a_future_date_is_specified_Then_the_provider_should_throw_an_exception()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var bankApiClient = new Mock<IBankApiClient>();
        var logger = new Mock<ILogger<IExchangeRateProvider>>();
        var now = new DateTimeOffset(2024, 6, 8, 0, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(now);

        var provider = new ExchangeRateProvider(bankApiClient.Object, cache, logger.Object, timeProvider);

        // Act/Assert
        await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await provider.GetExchangeRatesAsync(
                    new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c)), now.AddDays(1));
            });
    }
}
