
using Application.Services.Implementations;
using Application.Services.Interfaces;
using AutoFixture;
using Domain.Core;
using Domain.Model;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Application.Services.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Fixture fixture;
        private readonly IExchangeRateProvider provider;
        private readonly ICNBGateway CNBgateway;

        public ExchangeRateProviderTests()
        {
            fixture = new Fixture();
            CNBgateway = Substitute.For<ICNBGateway>();
            provider = new ExchangeRateProvider(CNBgateway);
        }

        [Fact]
        public void GetExchangeRatesForValidCurrency_GetWithSuccess_RetrieveExchangeRatesForSpeciefiedCurrency()
        {
            //Arrange
            this.InitializeExchangeRateCache();
            //Act
            var exchangeRates = provider.GetExchangeRates(currencies);

            //Assert
            exchangeRates.Should().BeOfType<List<ExchangeRate>>();
            exchangeRates.Should().NotBeEmpty();
        }

        [Fact]
        public void GetExchangeRatesForUnkownCurrency_GetWithSuccess_RetrieveEmptyListOfCurrencies()
        {
            //Arrange
            this.InitializeExchangeRateCache();
            //Act
            var exchangeRates = provider.GetExchangeRates(new List<Currency> { new Currency("ABC") });

            //Assert
            exchangeRates.Should().BeOfType<List<ExchangeRate>>();
            exchangeRates.Should().BeEmpty();
        }

        [Theory]
        [InlineData("USD", "CZK", 24.420)]
        [InlineData("EUR", "CZK", 24.54)]
        [InlineData("INR", "CZK", 30.687)]
        public void ToReferenceCurrencyCode_FROMOtherValidCurrenciesT0CZK_RetrieveValueInOtherCurrencies(string currencyCode, string referenceCurrencyCode, decimal expected)
        {
            //Arrange
            this.InitializeExchangeRateCache();

            //Act
            var exchangeRates = provider.ToReferenceCurrencyCode(currencyCode, referenceCurrencyCode);

            //Assert
            exchangeRates.Should().Be(expected);
        }

        [Theory]
        [InlineData("PTK", "CZK", 0)]
        [InlineData("UPT", "CZK", 0)]
        public void ToReferenceCurrencyCode_FromCZKToOtherNotValidCurrencies_RetrieveEmpty(string currencyCode, string referenceCurrencyCode, decimal expected)
        {
            //Arrange
            this.InitializeExchangeRateCache();

            //Act
            var exchangeRates = provider.ToReferenceCurrencyCode(currencyCode, referenceCurrencyCode);

            //Assert
            exchangeRates.Should().Be(expected);
        }

        [Theory]
        [InlineData("USD", "CZK", 1 / 24.420)]
        [InlineData("EUR", "CZK", 1 / 24.54)]
        [InlineData("INR", "CZK", 1 / 30.687)]
        public void FromReferenceCurrencyCode_FromCZKToOtherValidCurrencies_RetrieveValueInOtherCurrencies(string currencyCode, string referenceCurrencyCode, decimal expected)
        {
            //Arrange
            this.InitializeExchangeRateCache();

            //Act
            var exchangeRates = provider.FromReferenceCurrencyCode(currencyCode, referenceCurrencyCode);

            //Assert
            double precision = 0.1;

            exchangeRates.Should().BeApproximately(expected, new decimal(precision));
        }

        [Theory]
        [InlineData("PTK", "CZK", 0)]
        [InlineData("UPT", "CZK", 0)]
        public void FromReferenceCurrencyCode_FromCZKToNotValidCurrencies_RetrieveEmpty(string currencyCode, string referenceCurrencyCode, decimal expected)
        {
            //Arrange
            this.InitializeExchangeRateCache();

            //Act
            var exchangeRates = provider.ToReferenceCurrencyCode(currencyCode, referenceCurrencyCode);

            //Assert
            exchangeRates.Should().Be(expected);
        }

        [Theory]
        [InlineData("EUR", "USD", 1.02)]
        [InlineData("AUD", "EUR", 0.68)]
        public void Convert_FromValidCurrencyToValidCurrency_RetrieveConversion(string sourceCurrencyCode, string targetCurrencyCode, decimal expected)
        {
            //Arrange
            var sourceCurrency = new Currency(sourceCurrencyCode);
            var targetCurrency = new Currency(targetCurrencyCode);
            double precision = 0.1;
            this.InitializeExchangeRateCache();

            //Act
            var exchangeRates = provider.Convert(sourceCurrency, targetCurrency);

            //Assert
            exchangeRates.Value.Should().BeApproximately(expected,new decimal(precision));
        }

        public void InitializeExchangeRateCache()
        {
            Dictionary<string, List<ExchangeRate>> Data = new Dictionary<string, List<ExchangeRate>>()
            {
                {"CZK", new List<ExchangeRate>
                    {
                        new ExchangeRate(new Currency("AUD"),new Currency("CZK"),new decimal(16.689)),
                        new ExchangeRate(new Currency("BRL"),new Currency("CZK"),new decimal(4.714)),
                        new ExchangeRate(new Currency("BNG"),new Currency("CZK"),new decimal(12.547)),
                        new ExchangeRate(new Currency("CNY"),new Currency("CZK"),new decimal(3.529)),
                        new ExchangeRate(new Currency("DKK"),new Currency("CZK"),new decimal(3.30)),
                        new ExchangeRate(new Currency("EUR"),new Currency("CZK"),new decimal(24.54)),
                        new ExchangeRate(new Currency("PHP"),new Currency("CZK"),new decimal(42.978)),
                        new ExchangeRate(new Currency("HKD"),new Currency("CZK"),new decimal(3.111)),
                        new ExchangeRate(new Currency("HRK"),new Currency("CZK"),new decimal(3.261)),
                        new ExchangeRate(new Currency("INR"),new Currency("CZK"),new decimal(30.687)),
                        new ExchangeRate(new Currency("IDR"),new Currency("CZK"),new decimal(1.647)),
                        new ExchangeRate(new Currency("ISK"),new Currency("CZK"),new decimal(17.417)),
                        new ExchangeRate(new Currency("ILS"),new Currency("CZK"),new decimal(7.127)),
                        new ExchangeRate(new Currency("JPY"),new Currency("CZK"),new decimal(17.124)),
                        new ExchangeRate(new Currency("ZAR"),new Currency("CZK"),new decimal(7.412)),
                        new ExchangeRate(new Currency("CAD"),new Currency("CZK"),new decimal(18.775)),
                        new ExchangeRate(new Currency("KRW"),new Currency("CZK"),new decimal(1.771)),
                        new ExchangeRate(new Currency("HUF"),new Currency("CZK"),new decimal(6.191)),
                        new ExchangeRate(new Currency("MYR"),new Currency("CZK"),new decimal(5.429)),
                        new ExchangeRate(new Currency("MXN"),new Currency("CZK"),new decimal(1.228)),
                        new ExchangeRate(new Currency("XDR"),new Currency("CZK"),new decimal(31.680)),
                        new ExchangeRate(new Currency("NOK"),new Currency("CZK"),new decimal(2.458)),
                        new ExchangeRate(new Currency("NZD"),new Currency("CZK"),new decimal(14.910)),
                        new ExchangeRate(new Currency("PLN"),new Currency("CZK"),new decimal(5.198)),
                        new ExchangeRate(new Currency("RON"),new Currency("CZK"),new decimal(5.006)),
                        new ExchangeRate(new Currency("SGD"),new Currency("CZK"),new decimal(17.451)),
                        new ExchangeRate(new Currency("SEK"),new Currency("CZK"),new decimal(2.301)),
                        new ExchangeRate(new Currency("CHF"),new Currency("CZK"),new decimal(25.409)),
                        new ExchangeRate(new Currency("THB"),new Currency("CZK"),new decimal(67.216)),
                        new ExchangeRate(new Currency("TRY"),new Currency("CZK"),new decimal(1.339)),
                        new ExchangeRate(new Currency("USD"),new Currency("CZK"),new decimal(24.420)),
                        new ExchangeRate(new Currency("GBP"),new Currency("CZK"),new decimal(28.253))
                    }
                }
            };

            ExchangeRateCache.Data = Data;
        }

        public static IEnumerable<Currency> currencies = new[]
         {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
    }
}