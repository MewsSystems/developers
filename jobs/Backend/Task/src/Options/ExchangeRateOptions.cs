using System;

namespace ExchangeRateUpdater.Options
{
    public class ExchangeRateOptions
    {
        public required string BaseUrl { get; set; }
        public required string BaseCurrency { get; set; }
        public required string[] CurrenciesToWatch { get; set; } = [];
        public string BackupFilePath { get; set; } = "rates-backup.txt";
        public required HttpClientOptions HttpClient { get; set; }
    }
} 