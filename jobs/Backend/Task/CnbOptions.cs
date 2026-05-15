namespace ExchangeRateUpdater
{
    public class CnbOptions 
    {
        public const string SectionName = "Cnb";

        public string DailyKurzUrl { get; set; }
        public int TotalTimeoutSeconds { get; set; } = 10;
        public int AttemptTimeoutSeconds { get; set; } = 3;
        public int RetryCount { get; set; } = 2;
        public int RetryDelayMilliseconds { get; set; } = 250;
    }
}