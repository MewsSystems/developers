using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Configuration
{
    public class CnbApiOptions
    {
        public const string SectionName = "CnbApi";

        [Required]
        [Url]
        public string BaseUrl { get; set; } = string.Empty;

        [Required]
        public string DailyRatesPath { get; set; } = string.Empty;

        [Range(1, 60)]
        public int RequestTimeoutSeconds { get; set; } = 15;
    }
}