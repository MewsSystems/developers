using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services.Interfaces;

namespace ExchangeRateUpdater.Services.Backup
{
    public class BackupFileDataProvider : IExchangeRateDataProvider
    {
        private readonly ExchangeRateOptions _options;
        private readonly ILogger<BackupFileDataProvider> _logger;

        public BackupFileDataProvider(
            IOptions<ExchangeRateOptions> options,
            ILogger<BackupFileDataProvider> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(_options.BackupFilePath))
            {
                throw new ArgumentException("BackupFilePath must be specified in options", nameof(options));
            }
        }

        public async Task<string> GetRawDataAsync()
        {
            try
            {
                _logger.LogInformation("Using data provider: Backup File");
                _logger.LogDebug("Attempting to read data from file: {Path}", _options.BackupFilePath);
                
                if (!File.Exists(_options.BackupFilePath))
                {
                    throw new FileNotFoundException("Backup file does not exist", _options.BackupFilePath);
                }

                var data = await File.ReadAllTextAsync(_options.BackupFilePath);
                
                if (string.IsNullOrWhiteSpace(data))
                {
                    throw new InvalidOperationException("Backup file is empty");
                }

                // Extract and validate backup date
                var backupDate = ExtractDateFromData(data);
                var currentDate = DateTime.Now.Date;

                if (backupDate.HasValue)
                {
                    if (backupDate.Value.Date != currentDate)
                    {
                        _logger.LogWarning(
                            "Backup data is not from current date. Backup date: {BackupDate}, Current date: {CurrentDate}",
                            backupDate.Value.ToString("yyyy-MM-dd"),
                            currentDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        _logger.LogDebug("Backup data date: {BackupDate}", backupDate.Value.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    _logger.LogWarning("Could not determine backup data date");
                }

                _logger.LogDebug("Data successfully read from backup file");
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading data from backup file");
                throw;
            }
        }

        private DateTime? ExtractDateFromData(string data)
        {
            try
            {
                _logger.LogDebug("Attempting to extract date from first line of data");
                var firstLine = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                
                // Format is: "dd MMM yyyy #nnn"
                var datePart = firstLine.Split('#')[0].Trim();
                _logger.LogDebug("Date part found: {DatePart}", datePart);

                // Convert month name to number
                var parts = datePart.Split(' ');
                if (parts.Length != 3)
                {
                    _logger.LogWarning("Invalid date format in first line: {Line}", firstLine);
                    return null;
                }

                var day = int.Parse(parts[0]);
                var month = GetMonthNumber(parts[1]);
                var year = int.Parse(parts[2]);

                if (month == 0)
                {
                    _logger.LogWarning("Invalid month name in date: {Month}", parts[1]);
                    return null;
                }

                var date = new DateTime(year, month, day);
                _logger.LogDebug("Date successfully extracted: {Date}", date.ToString("yyyy-MM-dd"));
                return date;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error extracting date from data. First line does not match expected format");
                return null;
            }
        }

        private int GetMonthNumber(string monthName)
        {
            return monthName.ToLower() switch
            {
                "jan" => 1,
                "feb" => 2,
                "mar" => 3,
                "apr" => 4,
                "may" => 5,
                "jun" => 6,
                "jul" => 7,
                "aug" => 8,
                "sep" => 9,
                "oct" => 10,
                "nov" => 11,
                "dec" => 12,
                _ => 0
            };
        }
    }
} 