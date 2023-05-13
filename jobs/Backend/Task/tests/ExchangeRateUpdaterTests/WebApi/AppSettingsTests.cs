using ExchangeRateUpdater.WebApi;
using System;
using Xunit;

namespace ExchangeRateUpdaterTests.WebApi
{
    public class AppSettingsTests
    {
        [Fact]
        public void AppSettings_When_EmptyExchangeRateProviderUrl_Then_ThrowsException()
        {
            // Arrange
            var appSettings = new AppSettings
            {
                ExchangeRateProviderUrl = string.Empty,
                SourceCurrency = "SourceCurrency"
            };

            // Act & Assert
            Assert.Throws<Exception>(() => appSettings.EnsureIsOk());
        }

        [Fact]
        public void AppSettings_When_EmptySourceCurrency_Then_ThrowsException()
        {
            // Arrange
            var appSettings = new AppSettings
            {
                ExchangeRateProviderUrl = "Url",
                SourceCurrency = string.Empty
            };

            // Act & Assert
            Assert.Throws<Exception>(() => appSettings.EnsureIsOk());
        }

        [Fact]
        public void AppSettings_When_CorrectConfiguration_Then_DoesNotThrowException()
        {
            // Arrange
            var appSettings = new AppSettings
            {
                ExchangeRateProviderUrl = "Url",
                SourceCurrency = "SourceCurrency"
            };

            // Act & Assert
            appSettings.EnsureIsOk();
        }
    }
}
