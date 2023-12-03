﻿using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Sinks.InMemory;

namespace Adapter.ExchangeRateProvider.CzechNationalBank.Tests.Integration;

[TestFixture]
internal class GetExchangeRateForCurrenciesAsyncTests
{
    private TestHttpClientFactory? _httpClientFactory;
    private ILogger? _logger;

    [Test]
    public async Task GivenValidExchangeOrder_WhenCallingCzechNationalBankToGetExchange_ShouldReturnAListOfUnitExchangeRatesSortedByDate()
    {
        // act
        var sut = CreateSut();

        // assert
        var result = await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(2023, 1, 1), new DateTime(2023, 1, 2));
        result.ToList().Count.Should().BeGreaterThan(0);
    }

    private IExchangeRateProviderRepository CreateSut()
    {
        return new CzechNationalBankRepository(_httpClientFactory, _logger);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory("https://www.cnb.cz/en/");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClientFactory?.Dispose();
    }
}