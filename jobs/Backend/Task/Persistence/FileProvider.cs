using Microsoft.Extensions.Logging;
using System;

namespace ExchangeRateUpdater.Persistence
{
    public class FileProvider : IFileProvider
    {
        private readonly string _filename;
        private readonly ILogger<FileProvider> _logger;

        public FileProvider(string filename, ILogger<FileProvider> logger)
        {
            if(string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            _filename = filename;
            _logger = logger;
        }

        public bool TryGetFileContent(out string content)
        {
            try
            {
                content = System.IO.File.ReadAllText(_filename);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while opening file {_filename}. Exception: {e.Message}");
                content = string.Empty;
                return false;
            }

            return true;
        }

        public bool SaveFile(string content)
        {
            try
            {
                System.IO.File.WriteAllText(_filename, content);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while opening file {_filename}. Exception: {e.Message}");
                return false;
            }

            return true;
        }
    }
}
