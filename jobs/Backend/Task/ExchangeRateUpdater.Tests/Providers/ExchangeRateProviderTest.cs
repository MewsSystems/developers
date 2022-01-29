using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Dtos;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Tests.Constants;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Providers
{
    [TestFixture]
    public class ExchangeRateProviderTest : TestBase
    {
        ExchangeRateProvider _exchangeRateProvider;
        
        [SetUp]
        public void SetUp()
        {
            _exchangeRateProvider = new ExchangeRateProvider(Mock.CzechNationalBankApiClient.Object);
        }

        [Test]
        public void GetExchangeRates_ShouldFilterExchangeRates_WhenSpecificCurrenciesAreSpecified()
        {
            // Given
            var exchangeRateDtos = new List<ExchangeRateDto>
            {
                new() { Currency = ExchangeRateConstants.Currency1, Rate = ExchangeRateConstants.Rate1, Amount = ExchangeRateConstants.Amount2 },
                new() { Currency = ExchangeRateConstants.Currency2, Rate = ExchangeRateConstants.Rate2, Amount = ExchangeRateConstants.Amount1 },
                new() { Currency = ExchangeRateConstants.Currency3, Rate = ExchangeRateConstants.Rate3, Amount = ExchangeRateConstants.Amount2 },
                new() { Currency = ExchangeRateConstants.Currency4, Rate = ExchangeRateConstants.Rate4, Amount = ExchangeRateConstants.Amount3 },
                new() { Currency = ExchangeRateConstants.Currency5, Rate = ExchangeRateConstants.Rate5, Amount = ExchangeRateConstants.Amount2 },
            };
            Mock.CzechNationalBankApiClient
                .Setup(c => c.GetExchangeRatesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(exchangeRateDtos);
                
            var currencies = new List<Currency>
            {
                new("USD"),
                new("EUR"),
                new("JPY")
            };

            var expectedExchangeRates = new List<ExchangeRate>
            {
                new(ExchangeRateConstants.Currency1, ExchangeRateConstants.Currency6, ExchangeRateConstants.Rate1),
                new(ExchangeRateConstants.Currency3, ExchangeRateConstants.Currency6, ExchangeRateConstants.Rate3),
                new(
                    ExchangeRateConstants.Currency4,
                    ExchangeRateConstants.Currency6,
                    ExchangeRateConstants.Rate4 / (decimal)ExchangeRateConstants.Amount3)
            };
            
            // When
            var resultExchangeRates = _exchangeRateProvider.GetExchangeRates(currencies);

            // Then
            CompareTwoCollectionsDeeply<ExchangeRate>(
                expectedExchangeRates,
                resultExchangeRates,
                new ExchangeRateComparer()).Should().BeTrue();
            Mock.CzechNationalBankApiClient
                .Verify(c => c.GetExchangeRatesAsync(It.IsAny<DateTime>()), Times.Once);
        }
        
        class ExchangeRateComparer : IComparer<ExchangeRate>
        {
            public int Compare(ExchangeRate x, ExchangeRate y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;

                var currencyComparer = new CurrencyComparer();
                
                var sourceCurrency = currencyComparer.Compare(x.SourceCurrency, y.SourceCurrency);
                if (sourceCurrency != 0) return sourceCurrency;
                
                var targetCurrency = currencyComparer.Compare(x.TargetCurrency, y.TargetCurrency);
                if (targetCurrency != 0) return targetCurrency;
                
                return x.Value.CompareTo(y.Value);
            }
        }
        
        class CurrencyComparer : IComparer<Currency>
        {
            public int Compare(Currency x, Currency y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Code, y.Code, StringComparison.Ordinal);
            }
        }
    }
}