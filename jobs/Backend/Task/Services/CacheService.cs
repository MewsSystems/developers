using System.IO.Abstractions;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using Serilog;

namespace ExchangeRateUpdater.Services
{
    public class CacheService : ICacheService
    {
        private readonly IFileSystem _fileSystem;

        public CacheService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private string GetCacheFilePath(string cacheKey)
        {
            return _fileSystem.Path.Combine("Cache", $"{cacheKey}.cache");
        }

        public async Task<string> GetCachedDataAsync(string cacheKey)
        {
            string filePath = GetCacheFilePath(cacheKey);
            if (_fileSystem.File.Exists(filePath))
            {
                Log.Debug("Using cached data from {FilePath}", filePath);
                return await _fileSystem.File.ReadAllTextAsync(filePath);
            }
            Log.Debug("Cache miss for {FilePath}", filePath);
            return null;
        }

        public async Task SaveDataToCacheAsync(string cacheKey, string data)
        {
            string filePath = GetCacheFilePath(cacheKey);
            string directoryPath = _fileSystem.Path.GetDirectoryName(filePath);
            if (!_fileSystem.Directory.Exists(directoryPath))
            {
                _fileSystem.Directory.CreateDirectory(directoryPath);
                Log.Debug("Created directory for cache at {DirectoryPath}", directoryPath);
            }
            await _fileSystem.File.WriteAllTextAsync(filePath, data);
            Log.Debug("Saved data to cache at {FilePath}", filePath);
        }
    }
}
