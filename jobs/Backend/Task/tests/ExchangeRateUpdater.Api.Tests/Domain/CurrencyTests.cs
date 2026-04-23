using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;

namespace ExchangeRateUpdater.Api.Tests.Domain;

public class CurrencyTests
{
    public class Create
    {
        [Fact]
        public void Create_WithValidCode_ReturnsSuccessWithCurrency()
        {
            var result = Currency.Create("USD");

            Assert.True(result.IsSuccess);
            Assert.Equal("USD", result.Value.Code);
        }

        [Theory]
        [InlineData("usd", "USD")]
        [InlineData("eUr", "EUR")]
        [InlineData("GbP", "GBP")]
        public void Create_WithAnyCase_NormalizesToUppercase(string input, string expected)
        {
            var result = Currency.Create(input);

            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Value.Code);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Create_WithEmptyOrWhitespace_ReturnsFailure(string code)
        {
            var result = Currency.Create(code);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationCurrencyCodeNullOrWhitespace, result.Error.Code);
        }

        [Theory]
        [InlineData("US")]
        [InlineData("USDX")]
        [InlineData("A")]
        public void Create_WithInvalidLength_ReturnsFailure(string code)
        {
            var result = Currency.Create(code);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationCurrencyCodeInvalidLength, result.Error.Code);
        }

        [Theory]
        [InlineData("12A")]
        [InlineData("U$D")]
        [InlineData("US1")]
        public void Create_WithNonAlphaCharacters_ReturnsFailure(string code)
        {
            var result = Currency.Create(code);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationCurrencyCodeInvalidCharacters, result.Error.Code);
        }
    }

    public class Equality
    {
        [Fact]
        public void Equals_WithSameCode_ReturnsTrue()
        {
            var a = Currency.Create("USD").Value;
            var b = Currency.Create("USD").Value;

            Assert.Equal(a, b);
            Assert.True(a == b);
        }

        [Fact]
        public void Equals_WithDifferentCase_ReturnsTrue()
        {
            var a = Currency.Create("usd").Value;
            var b = Currency.Create("USD").Value;

            Assert.Equal(a, b);
        }

        [Fact]
        public void Equals_WithDifferentCode_ReturnsFalse()
        {
            var a = Currency.Create("USD").Value;
            var b = Currency.Create("EUR").Value;

            Assert.NotEqual(a, b);
            Assert.True(a != b);
        }

        [Fact]
        public void GetHashCode_WithEqualCurrencies_ReturnsSameHashCode()
        {
            var a = Currency.Create("usd").Value;
            var b = Currency.Create("USD").Value;

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }

    public class ToStringMethod
    {
        [Fact]
        public void ReturnsCode()
        {
            var currency = Currency.Create("CZK").Value;

            Assert.Equal("CZK", currency.ToString());
        }
    }
}
