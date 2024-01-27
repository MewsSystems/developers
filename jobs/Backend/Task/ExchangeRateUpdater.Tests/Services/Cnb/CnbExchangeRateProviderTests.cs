using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Tests.Doubles;
using ExchangeRateUpdater.Tests.Services.Cnb.Doubles;
using Mews.Integrations.Cnb.Contracts.Models;
using Mews.Integrations.Cnb.Models;
using Mews.Integrations.Cnb.Services;
using Mews.Shared.Temporal;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services.Cnb;

public class CnbExchangeRateProviderTests
{
    private static IClock Clock => 
        new StoppedClock { Now = new DateTimeOffset(2024, 1, 27, 8, 0, 0, TimeSpan.Zero) };
    
    private static string DefaultValidFor => new DateTimeOffset(2024, 1, 27, 8, 0, 0, TimeSpan.Zero).ToString("yyyy-MM-dd");
    
    [Fact]
    public async Task ProvidesAllRequestedExchangeRates()
    {
        // Given
        var expectedClientResponse = CreateCnbClientResponse(DefaultValidFor);
        var cnbClientStub = CreateCnbClientStub(expectedClientResponse);
        var provider = InitializeProvider(cnbClientStub);
        var requestedCurrencyCodes = new[] { "USD", "JPY" };
        var requestedCurrencies = GetRequestedCurrencies(requestedCurrencyCodes);
        
        // When
        var exchangeRatesResult = await provider.GetExchangeRatesAsync(requestedCurrencies, Clock.Now, CancellationToken.None);
        
        // Then
        foreach (var expected in expectedClientResponse.Rates.Where(r => requestedCurrencyCodes.Contains(r.CurrencyCode)))
        {
            var result = exchangeRatesResult.Single(r => r.SourceCurrency.Code == expected.CurrencyCode);
            AssertExchangeRate(expected, result);
        }

    }
    
    [Fact]
    public async Task IgnoresRequestedExchangeRatesThatAreNotProvidedByCnbApi()
    {
        // Given
        var expectedResponse = CreateCnbClientResponse(DefaultValidFor);
        var cnbClientStub = CreateCnbClientStub(expectedResponse);
        var provider = InitializeProvider(cnbClientStub);
        var requestedCurrencies = GetRequestedCurrencies("USD", "EUR", "JPY", "RUB");
        
        // When
        var exchangeRatesResult = await provider.GetExchangeRatesAsync(requestedCurrencies, Clock.Now, CancellationToken.None);
        
        // Then
        Assert.Collection(
            exchangeRatesResult,
            r => Assert.Equal("USD", r.SourceCurrency.Code),
            r => Assert.Equal("EUR", r.SourceCurrency.Code),
            r => Assert.Equal("JPY", r.SourceCurrency.Code));

    }

    [Fact]
    public async Task CorrectlyFormatsExchangeRate()
    {
        // Given
        var expectedResponse = CreateCnbClientResponse(DefaultValidFor);
        var cnbClientStub = CreateCnbClientStub(expectedResponse);
        var provider = InitializeProvider(cnbClientStub);
        var requestedCurrencies = GetRequestedCurrencies("USD");
        
        // When
        var exchangeRatesResult = await provider.GetExchangeRatesAsync(requestedCurrencies, Clock.Now, CancellationToken.None);
        
        // Then
        var expectedUsd = expectedResponse.Rates.Single(r => r.CurrencyCode == "USD");
        var exchangeRateResult = Assert.Single(exchangeRatesResult);
        Assert.Equal($"USD/CZK={expectedUsd.Rate}", exchangeRateResult.ToString());
    }
    
    private static void AssertExchangeRate(CnbClientExchangeRateResponseItem expected, ExchangeRate result)
    {
        Assert.Equal(expected.CurrencyCode, result.SourceCurrency.Code);
        Assert.Equal(expected.Rate / expected.Amount, result.Value);
        Assert.Equal(expected.ValidFor, result.ValidFor.ToString("yyyy-MM-dd"));
    }

    private static CnbClientStub CreateCnbClientStub(CnbClientExchangeRateResponse sampleResponse)
    {
        return new CnbClientStub(sampleResponse);
    }

    private static CnbClientExchangeRateResponse CreateCnbClientResponse(string validFor)
    {
        var sampleResponse = new CnbClientExchangeRateResponse
        {
            Rates = new List<CnbClientExchangeRateResponseItem>
            {
                new() { Currency = "dollar", CurrencyCode = "USD", Amount = 1, ValidFor = validFor, Rate = 22.752m },
                new() { Currency = "euro", CurrencyCode = "EUR", Amount = 1, ValidFor = validFor, Rate = 24.74m },
                new() { Currency = "yen", CurrencyCode = "JPY", Amount = 100, ValidFor = validFor, Rate = 15.401m }
            }
        };
        return sampleResponse;
    }

    private static IReadOnlyList<Currency> GetRequestedCurrencies(params string[] currencies)
    {
        return currencies.Select(c => new Currency(c)).ToList();
    }
    
    private static CnbExchangeRateProvider InitializeProvider(CnbClientStub cnbClientStub)
    {
        return new CnbExchangeRateProvider(cnbClientStub, NullLogger<CnbExchangeRateProvider>.Instance);
    }
}
