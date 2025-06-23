using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class InMemoryExchangeRateRepositoryTests
    {
        [Fact]
        public async Task GetExchangeRates_ReturnsNull_WhenKeyNotFound()
        {
            // Arrange
            var repository = new InMemoryExchangeRateRepository();
            string key = "NonExistingKey";

            // Act
            var result = await repository.GetExchangeRates(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveExchangeRates_ReturnsTrue_WhenDatasetNotEmpty()
        {
            // Arrange
            var repository = new InMemoryExchangeRateRepository();
            string key = "TestKey";
            var dataset = new ExchangeRateDataset(
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new ExchangeRateData("Dollar", "USD", "US", 1.0m),
                    new ExchangeRateData("Euro", "EUR", "Europe", 0.85m)
                ],
                ExchangeRateDataset.Channel.Direct
            );

            // Act
            var result = await repository.SaveExchangeRates(key, dataset);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SaveExchangeRates_DoesNotSave_WhenDatasetIsEmpty()
        {
            // Arrange
            var repository = new InMemoryExchangeRateRepository();
            string key = "TestKey";
            var emptyDataset = new ExchangeRateDataset(DateOnly.FromDateTime(DateTime.Today), [], ExchangeRateDataset.Channel.Direct);

            // Act
            await repository.SaveExchangeRates(key, emptyDataset);
            var result = await repository.GetExchangeRates(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveExchangeRates_OverwritesExistingDataset_WhenKeyAlreadyExists()
        {
            // Arrange
            var repository = new InMemoryExchangeRateRepository();
            string key = "TestKey";
            var initialDataset = new ExchangeRateDataset(
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new ExchangeRateData("Dollar", "USD", "US", 1.0m),
                    new ExchangeRateData("Euro", "EUR", "Europe", 0.85m)
                ],
                ExchangeRateDataset.Channel.Direct
            );
            await repository.SaveExchangeRates(key, initialDataset);

            var newDataset = new ExchangeRateDataset(
                DateOnly.FromDateTime(DateTime.Today),
                [
                    new ExchangeRateData("Pound", "GBP", "UK", 0.75m),
                    new ExchangeRateData("Yen", "JPY", "Japan", 110.0m)
                ],
                ExchangeRateDataset.Channel.Worker
            );

            // Act
            await repository.SaveExchangeRates(key, newDataset);
            var result = await repository.GetExchangeRates(key);

            // Assert
            Assert.Equal(newDataset, result);
        }
    }
}
