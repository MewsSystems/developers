using ExchangeRateUpdater.Services;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests.Services
{
    public class CacheServiceTests
    {
        [Fact]
        public async Task GetCachedDataAsync_ReturnsData_WhenFileExists()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var cacheKey = "testKey";
            var filePath = Path.Combine("Cache", $"{cacheKey}.cache");
            var expectedData = "Cached data";
            mockFileSystem.AddFile(filePath, new MockFileData(expectedData));

            var cacheService = new CacheService(mockFileSystem);

            // Act
            var result = await cacheService.GetCachedDataAsync(cacheKey);

            // Assert
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public async Task GetCachedDataAsync_ReturnsNull_WhenFileDoesNotExist()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var cacheKey = "testKey";
            var cacheService = new CacheService(mockFileSystem);

            // Act
            var result = await cacheService.GetCachedDataAsync(cacheKey);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveDataToCacheAsync_CreatesDirectoryAndFile_WhenTheyDoNotExist()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var cacheKey = "testKey";
            var data = "New cached data";
            var filePath = Path.Combine("Cache", $"{cacheKey}.cache");
            var directoryPath = Path.GetDirectoryName(filePath);
            var cacheService = new CacheService(mockFileSystem);

            // Act
            await cacheService.SaveDataToCacheAsync(cacheKey, data);

            // Assert
            Assert.True(mockFileSystem.FileExists(filePath));
            Assert.True(mockFileSystem.Directory.Exists(directoryPath));
            Assert.Equal(data, mockFileSystem.File.ReadAllText(filePath));
        }
    }
}
