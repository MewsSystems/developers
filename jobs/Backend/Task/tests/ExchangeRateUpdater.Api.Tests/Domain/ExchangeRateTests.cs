using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;

namespace ExchangeRateUpdater.Api.Tests.Domain;

public class ExchangeRateTests
{
    public class Create
    {
        private static readonly DateOnly DefaultDate = new(2025, 6, 15);

        [Fact]
        public void Create_WithValidArguments_ReturnsSuccessWithExchangeRate()
        {
            var result = ExchangeRate.Create("USD", "CZK", 23.45m, 1, DefaultDate);

            Assert.True(result.IsSuccess);
            Assert.Equal("USD", result.Value.SourceCurrency.Code);
            Assert.Equal("CZK", result.Value.TargetCurrency.Code);
            Assert.Equal(23.45m, result.Value.Rate);
            Assert.Equal(1, result.Value.Amount);
            Assert.Equal(DefaultDate, result.Value.ValidFor);
        }

        [Fact]
        public void Create_WithZeroRate_ReturnsSuccess()
        {
            var result = ExchangeRate.Create("USD", "CZK", 0m, 1, DefaultDate);

            Assert.True(result.IsSuccess);
            Assert.Equal(0m, result.Value.Rate);
        }

        [Fact]
        public void Create_WithNegativeRate_ReturnsFailure()
        {
            var result = ExchangeRate.Create("USD", "CZK", -1m, 1, DefaultDate);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationInvalidRate, result.Error.Code);
        }

        [Fact]
        public void Create_WithInvalidSourceCurrency_ReturnsFailure()
        {
            var result = ExchangeRate.Create("US", "CZK", 23.45m, 1, DefaultDate);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationCurrencyCodeInvalidLength, result.Error.Code);
        }

        [Fact]
        public void Create_WithInvalidTargetCurrency_ReturnsFailure()
        {
            var result = ExchangeRate.Create("USD", "12A", 23.45m, 1, DefaultDate);

            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.ValidationCurrencyCodeInvalidCharacters, result.Error.Code);
        }
    }

    public class ToStringMethod
    {
        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            var result = ExchangeRate.Create("USD", "CZK", 23.45m, 1, new DateOnly(2025, 6, 15));

            var str = result.Value.ToString();

            Assert.Contains("USD", str);
            Assert.Contains("CZK", str);
            Assert.Contains("23.45", str, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
