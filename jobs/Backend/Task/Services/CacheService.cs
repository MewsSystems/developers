using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace ExchangeRateUpdater.Services
{
    public class CacheService
    {
        private static string GetCacheFilePath(string cacheKey)
        {
            return Path.Combine("Cache", $"{cacheKey}.cache");
        }

        public async Task<string> GetCachedDataAsync(string cacheKey)
        {
            string filePath = GetCacheFilePath(cacheKey);
            if (File.Exists(filePath))
            {
                Log.Debug("Using cached data from {FilePath}", filePath);
                return await File.ReadAllTextAsync(filePath);
            }
            Log.Debug("Cache miss for {FilePath}", filePath);
            return null;
        }

        public async Task SaveDataToCacheAsync(string cacheKey, string data)
        {
            string filePath = GetCacheFilePath(cacheKey);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Log.Debug("Created directory for cache at {DirectoryPath}", directoryPath);
            }
            await File.WriteAllTextAsync(filePath, data);
            Log.Debug("Saved data to cache at {FilePath}", filePath);
        }
    }
}