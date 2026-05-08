using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services.Interfaces;
using System.Linq;

namespace ExchangeRateUpdater.Services.CNB
{
    public class CNBHttpDataProvider : IExchangeRateDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateOptions _options;
        private readonly ILogger<CNBHttpDataProvider> _logger;

        public CNBHttpDataProvider(
            HttpClient httpClient,
            IOptions<ExchangeRateOptions> options,
            ILogger<CNBHttpDataProvider> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            {
                throw new ArgumentException("BaseUrl must be specified in options", nameof(options));
            }

            if (_options.CurrenciesToWatch == null || _options.CurrenciesToWatch.Length == 0)
                throw new ArgumentException("CurrenciesToWatch cannot be null or empty", nameof(options));

            if (_options.CurrenciesToWatch.Any(currency => string.IsNullOrWhiteSpace(currency)))
                throw new ArgumentException("CurrenciesToWatch contains invalid currency codes", nameof(options));

            // Ensure backup directory exists
            var backupDir = Path.GetDirectoryName(_options.BackupFilePath);
            if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }
        }

        public async Task<string> GetRawDataAsync()
        {
            try
            {
                _logger.LogInformation("Using data provider: CNB API");
                _logger.LogDebug("Attempting to fetch data from: {Url}", _options.BaseUrl);
                
                var response = await _httpClient.GetAsync(_options.BaseUrl);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(data))
                {
                    _logger.LogWarning("Received empty data from CNB");
                    throw new InvalidOperationException("Received empty data from CNB");
                }

                // Extract and validate data date
                var dataDate = ExtractDateFromData(data);
                var currentDate = DateTime.Now.Date;

                if (dataDate.HasValue)
                {
                    if (dataDate.Value.Date != currentDate)
                    {
                        _logger.LogWarning(
                            "CNB data is not from current date. Data date: {DataDate}, Current date: {CurrentDate}",
                            dataDate.Value.ToString("yyyy-MM-dd"),
                            currentDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        _logger.LogDebug("CNB data date: {DataDate}", dataDate.Value.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    _logger.LogWarning("Could not determine CNB data date");
                }

                _logger.LogDebug("Saving backup copy of the data");
                await SaveBackupAsync(data);
                
                return data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching data from CNB. Attempting to use backup data...");
                try
                {
                    var backupData = await LoadBackupAsync();
                    if (!string.IsNullOrWhiteSpace(backupData))
                    {
                        _logger.LogInformation("Using backup data due to CNB error");
                        return backupData;
                    }
                }
                catch (Exception backupEx)
                {
                    _logger.LogError(backupEx, "Error loading backup data");
                }

                throw new Exception("Could not connect to CNB server and no valid backup data available", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching exchange rates");
                throw;
            }
        }

        private async Task SaveBackupAsync(string data)
        {
            try
            {
                await File.WriteAllTextAsync(_options.BackupFilePath, data);
                _logger.LogDebug("Backup data successfully saved to: {Path}", _options.BackupFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to save backup data. This won't affect the current operation");
            }
        }

        private async Task<string> LoadBackupAsync()
        {
            if (File.Exists(_options.BackupFilePath))
            {
                var data = await File.ReadAllTextAsync(_options.BackupFilePath);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var backupDate = ExtractDateFromData(data);
                    if (backupDate.HasValue)
                    {
                        _logger.LogWarning(
                            "Using backup data from {BackupDate}",
                            backupDate.Value.ToString("yyyy-MM-dd"));
                    }
                    return data;
                }
            }
            return null;
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